// See https://aka.ms/new-console-template for more information
using System.Text.Json.Serialization;
using WazuhCommon.Models;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(WazuhResponse<ApiInfo>))]
[JsonSerializable(typeof(WazuhResponse<AgentListResponse>))]
[JsonSerializable(typeof(WazuhResponse<ItemListResponse>))]
public partial class ResponseGenerationContext : JsonSerializerContext
{

}