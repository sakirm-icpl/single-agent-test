using System.Net;

var host = Environment.GetEnvironmentVariable("WAZUH_SERVER_HOST");
var baseUrl = Environment.GetEnvironmentVariable("WAZUH_SERVER_URL");
var wazuhUsername = Environment.GetEnvironmentVariable("WAZUH_SERVER_USER");
var wazuhPassword = Environment.GetEnvironmentVariable("WAZUH_SERVER_PASSWORD");

Console.WriteLine($"Wazuh API: {baseUrl}");
HttpClientHandler handler = new()
{
    ClientCertificateOptions = ClientCertificateOption.Manual,
    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; },
    CookieContainer = new CookieContainer(),
    UseCookies = true
};

var client = new HttpClient(handler);
var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/auth/login");
request.Headers.Add("osd-xsrf", "osd-fetch");
request.Headers.Add("sec-ch-ua-platform", "\"Linux\"");
request.Headers.Add("Accept", "*/*");
request.Headers.Add("host", host);
request.Content = new StringContent($"{{\"username\":\"{wazuhUsername}\",\"password\":\"{wazuhPassword}\"}}", null, "application/json");
var response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();
ExtractResponseCookies(handler.CookieContainer, response);
Console.WriteLine(await response.Content.ReadAsStringAsync());

request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/api/login");
request.Headers.Add("sec-ch-ua", "\"Microsoft Edge\";v=\"119\", \"Chromium\";v=\"119\", \"Not?A_Brand\";v=\"24\"");
request.Headers.Add("DNT", "1");
request.Headers.Add("sec-ch-ua-mobile", "?0");
request.Headers.Add("Accept", "application/json, text/plain, */*");
request.Headers.Add("osd-xsrf", "kibana");
request.Headers.Add("sec-ch-ua-platform", "\"Linux\"");
request.Headers.Add("host", host);
request.Content = new StringContent("{\"idHost\":\"default\",\"force\":false}", null, "application/json");
response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();
ExtractResponseCookies(handler.CookieContainer, response);
Console.WriteLine(await response.Content.ReadAsStringAsync());

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, RequestGenerationContext.Default);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors();

var api = app.MapGroup("/api");
api.MapGet("/", async () =>
{
    var client = new HttpClient(handler);
    var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/api/request");
    request.Headers.Add("osd-xsrf", "kibana");
    request.Content = new StringContent("{\"method\": \"GET\", \"path\": \"/\", \"body\": { }, \"id\": \"default\"\r\n}", null, "application/json");
    var response = await client.SendAsync(request);
    response.EnsureSuccessStatusCode();
    return Results.Stream(response.Content.ReadAsStream(), "application/json");
});

api.MapPost("/request", async (HttpRequest apiRequest) =>
{
    using var reader = new StreamReader(apiRequest.Body);
    var body = await reader.ReadToEndAsync();

    var client = new HttpClient(handler);
    var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/api/request");
    request.Headers.Add("osd-xsrf", "kibana");
    request.Content = new StringContent(body, null, "application/json");
    var response = await client.SendAsync(request);
    response.EnsureSuccessStatusCode();
    return Results.Stream(response.Content.ReadAsStream(), "application/json");
});

app.Run();

static void ExtractResponseCookies(CookieContainer cookies, HttpResponseMessage response)
{
    var setCookieHeaders = response.Headers.GetValues("Set-Cookie");

    var requestUri = response?.RequestMessage?.RequestUri??new Uri("https://localhost/");

    if (setCookieHeaders != null)
    {
        cookies.SetCookies(requestUri, string.Join(",", setCookieHeaders));
    }
}