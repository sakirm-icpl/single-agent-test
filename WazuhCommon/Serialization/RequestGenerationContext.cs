// See https://aka.ms/new-console-template for more information
using System.Text.Json.Serialization;
using WazuhCommon.Models;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(RequestBase))]
[JsonSerializable(typeof(CommandRequest))]
public partial class RequestGenerationContext : JsonSerializerContext
{

}
