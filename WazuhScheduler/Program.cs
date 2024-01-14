// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using WazuhCommon.Models;
using WazuhCommon.Net;


var baseUrl = Environment.GetEnvironmentVariable("WAZUH_BASE_URL");
var basicAuth = Environment.GetEnvironmentVariable("WAZUH_BASIC_AUTH");

WazuhCommandQueueItem[] operations =
{
    new("q-quick-scan", "quick-scan0"),
    new("q-full-scan", "full-scan0"),
    new("q-isolation", "isolation0", LabelOperation.Add, "isolation"),
    new("q-unisolation", "unisolation0", LabelOperation.Remove, "isolation")
};

Console.WriteLine($"Wazuh API: {baseUrl}");

while (true)
{
    try
    {
        await ProcessAllQueues(baseUrl, basicAuth, operations);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
    
    for (int i = 15; i > 0; i--)
    {
        Console.WriteLine($"Waiting... {i}");
        await Task.Delay(1000);
    }
}

static async Task<EndPoint[]> GetActiveAgentsInGroup(string token, string baseUrl, string groupName)
{
    var agentListResponse = await WazuhApiGet(token, baseUrl, $"/agents?pretty=true&status=active&group={groupName}", ResponseGenerationContext.Default.WazuhResponseAgentListResponse);
    return agentListResponse.AffectedItems.ToArray();
}

static async Task<string[]> RemoveAgentsFromGroup(string token, string baseUrl, string groupName, params string[] agentIds)
{
    var agentListResponse = await WazuhApiDelete(token, baseUrl, $"/agents/group?pretty=false&wait_for_complete=false&agents_list={string.Join(",", agentIds)}&group_id={groupName}",
        ResponseGenerationContext.Default.WazuhResponseItemListResponse);

    return agentListResponse.AffectedItems.ToArray();
}

static async Task<string[]> AddAgentToGroup(string token, string baseUrl, string groupName, params string[] agentIds)
{
    var agentListResponse = await WazuhApiPut(token, baseUrl, $"/agents/group?pretty=false&wait_for_complete=false&agents_list={string.Join(",", agentIds)}&group_id={groupName}",
        RequestBase.Empty,
        RequestGenerationContext.Default.RequestBase,
        ResponseGenerationContext.Default.WazuhResponseItemListResponse);

    return agentListResponse.AffectedItems.ToArray();
}

static async Task<ItemListResponse> SendCommand(string token, string baseUrl, string command, params string[] agentIds)
{
    var commandRequest = new CommandRequest()
    {
        Command = command,
        Arguments = new List<string>(),
        Custom = false,
        Alert = new Alert()
    };

    var commandResponse = await WazuhApiPut(token, baseUrl, $"/active-response?agents_list={string.Join(",", agentIds)}&pretty=false&wait_for_complete=false",
        commandRequest,
        RequestGenerationContext.Default.CommandRequest,
        ResponseGenerationContext.Default.WazuhResponseItemListResponse);

    return commandResponse;
}

static async Task<string> GetWazuhToken(string baseUrl, string basicAuth)
{
    var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/security/user/authenticate?raw=true");
    request.Headers.Add("Accept", "application/json");
    request.Headers.Add("Authorization", $"Basic {basicAuth}");
    var response = await ConnectionManager.Client.SendAsync(request);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadAsStringAsync();
}

static async Task<T> WazuhApiGet<T>(string token, string baseUrl, string requestUrl, JsonTypeInfo<WazuhResponse<T>> responseTypeInfo) where T : ResponseBase
{
    var content = await ExchangeData(token, baseUrl, requestUrl, HttpMethod.Get, "");
    var obj = JsonSerializer.Deserialize(content, responseTypeInfo);

    // Print to console
    if (obj == null || obj.ErrorCode != 0)
    {
        throw new Exception($"Error getting API info: {obj}");
    }

    return obj.Data;
}

static async Task<E> WazuhApiPut<T, E>(string token, string baseUrl, string requestUrl, T body, JsonTypeInfo<T> requestTypeInfo, JsonTypeInfo<WazuhResponse<E>> responseTypeInfo)
    where T : RequestBase
    where E : ResponseBase
{
    var bodyContent = JsonSerializer.Serialize(body, requestTypeInfo);
    var content = await ExchangeData(token, baseUrl, requestUrl, HttpMethod.Put, bodyContent);
    var obj = JsonSerializer.Deserialize(content, responseTypeInfo);

    // Print to console
    if (obj == null || obj.ErrorCode != 0)
    {
        Console.WriteLine($"Error getting API info: {obj}");
    }

    return obj.Data;
}

static async Task<T> WazuhApiDelete<T>(string token, string baseUrl, string requestUrl, JsonTypeInfo<WazuhResponse<T>> responseTypeInfo) where T : ResponseBase
{
    var content = await ExchangeData(token, baseUrl, requestUrl, HttpMethod.Delete, "");
    var obj = JsonSerializer.Deserialize(content, responseTypeInfo);

    // Print to console
    if (obj == null || obj.ErrorCode != 0)
    {
        Console.WriteLine($"Error getting API info: {obj}");
    }

    return obj.Data;
}

static async Task<string> ExchangeData(string token, string baseUrl, string requestUrl, HttpMethod method, string body)
{
    Console.WriteLine($"{method} - {baseUrl}{requestUrl}");
    Console.WriteLine(body);

    var request = new HttpRequestMessage(method, $"{baseUrl}{requestUrl}");
    request.Headers.Add("Accept", "application/json");
    request.Headers.Add("Authorization", $"Bearer {token}");

    if (string.IsNullOrEmpty(body) == false)
    {
        request.Content = new StringContent(body, Encoding.UTF8, "application/json");
    }

    var response = await ConnectionManager.Client.SendAsync(request);

    response.EnsureSuccessStatusCode();
    var responseContent = await response.Content.ReadAsStringAsync();

    Console.WriteLine("RESPONSE:");
    Console.WriteLine(responseContent);
    Console.WriteLine(Environment.NewLine);
    return responseContent;
}

static async Task One(string token, string baseUrl)
{
    var agentListResponse = await WazuhApiGet(token, baseUrl, "/agents?pretty=true&group=default", ResponseGenerationContext.Default.WazuhResponseAgentListResponse);
    Console.WriteLine(agentListResponse);

    var activeAgents = await GetActiveAgentsInGroup(token, baseUrl, "default");
    Console.WriteLine($"Active AGENTS: {string.Join(",", activeAgents.Select(x => x.Id))}");

    var commandResponse = await SendCommand(token, baseUrl, "restart-wazuh", activeAgents.Select(x => x.Id).ToArray());
    Console.WriteLine($"Success AGENTS: {string.Join(",", commandResponse.AffectedItems)}");
    Console.WriteLine($"Failed AGENTS: {string.Join(",", commandResponse.FailedItems)}");
}

static async Task ProcessAllQueues(string baseUrl, string basicAuth, WazuhCommandQueueItem[] operations)
{
    //Get token from Wazuh API
    var token = await GetWazuhToken(baseUrl, basicAuth);
    Console.WriteLine(token);

    var apiInfo = await WazuhApiGet(token, baseUrl, "/", ResponseGenerationContext.Default.WazuhResponseApiInfo);
    Console.WriteLine(apiInfo);
    Console.WriteLine();
    Console.WriteLine();

    foreach (var item in operations)
    {
        await ProcessQueue(token, baseUrl, item);

        Console.WriteLine("Waiting 5 seconds...");
        await Task.Delay(5000);
    }
}

static async Task ProcessQueue(string token, string baseUrl, WazuhCommandQueueItem item)
{
    Console.WriteLine("=======================================");
    Console.WriteLine("Checking queue: " + item.QueueName);
    var activeAgents = await GetActiveAgentsInGroup(token, baseUrl, item.QueueName);
    Console.WriteLine($"Active agents in {item.QueueName}: {string.Join(",", activeAgents.Select(x => x.Id))}");

    if (activeAgents.Length == 0)
    {
        Console.WriteLine("No active agents in queue. Skipping further operations.");
        return;
    }


    foreach (var agent in activeAgents)
    {
        // Send command to agents
        var commandResponse = await SendCommand(token, baseUrl, item.Command, agent.Id);

        var successAgents = commandResponse.AffectedItems.ToArray();
        var failedAgents = commandResponse.FailedItems.ToArray();

        Console.WriteLine($"Success AGENTS: {string.Join(",", successAgents)}");
        Console.WriteLine($"Failed AGENTS: {string.Join(",", failedAgents.Select(x => x.ToString()))}");

        Console.WriteLine("Waiting for 5 seconds...");
        await Task.Delay(2000);

        // Remove agents from queue group
        var removedAgentsFromQueue = await RemoveAgentsFromGroup(token, baseUrl, item.QueueName, successAgents);
        Console.WriteLine($"Removed AGENTS from {item.QueueName}: {string.Join(",", removedAgentsFromQueue)}");

        if (item.LabelOperation == LabelOperation.Add)
        {
            // Add agents to isolation group
            var addedAgentsToLabel = await AddAgentToGroup(token, baseUrl, item.Label, successAgents);
            Console.WriteLine($"Added AGENTS to {item.Label}: {string.Join(",", addedAgentsToLabel)}");
        }
        else if (item.LabelOperation == LabelOperation.Remove)
        {
            // Remove agents from isolation group
            var removedAgentsFromLabel = await RemoveAgentsFromGroup(token, baseUrl, item.Label, successAgents);
            Console.WriteLine($"Removed AGENTS from {item.Label}: {string.Join(",", removedAgentsFromLabel)}");
        }

        Console.WriteLine("Waiting for 2 seconds...");
        await Task.Delay(2000);
    }
    
    /*
    // Send command to agents
    var commandResponse = await SendCommand(token, baseUrl, item.Command, activeAgents.Select(x => x.Id).ToArray());

    var successAgents = commandResponse.AffectedItems.ToArray();
    var failedAgents = commandResponse.FailedItems.ToArray();

    Console.WriteLine($"Success AGENTS: {string.Join(",", successAgents)}");
    Console.WriteLine($"Failed AGENTS: {string.Join(",", failedAgents.Select(x=> x.ToString()))}");

    Console.WriteLine("Waiting for 5 seconds...");
    await Task.Delay(5000);

    // Remove agents from queue group
    var removedAgentsFromQueue = await RemoveAgentsFromGroup(token, baseUrl, item.QueueName, successAgents);
    Console.WriteLine($"Removed AGENTS from {item.QueueName}: {string.Join(",", removedAgentsFromQueue)}");

    if (item.LabelOperation == LabelOperation.Add)
    {
        // Add agents to isolation group
        var addedAgentsToLabel = await AddAgentToGroup(token, baseUrl, item.Label, successAgents);
        Console.WriteLine($"Added AGENTS to {item.Label}: {string.Join(",", addedAgentsToLabel)}");
    }
    else if (item.LabelOperation == LabelOperation.Remove)
    {
        // Remove agents from isolation group
        var removedAgentsFromLabel = await RemoveAgentsFromGroup(token, baseUrl, item.Label, successAgents);
        Console.WriteLine($"Removed AGENTS from {item.Label}: {string.Join(",", removedAgentsFromLabel)}");
    }

    */
}