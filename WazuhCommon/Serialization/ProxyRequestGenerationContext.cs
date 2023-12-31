// See https://aka.ms/new-console-template for more information
using System.Text.Json.Serialization;
using WazuhCommon.Models;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(WazuhRequest<RequestBase>))]
[JsonSerializable(typeof(WazuhRequest<CommandRequest>))]
public partial class ProxyRequestGenerationContext : JsonSerializerContext
{

}