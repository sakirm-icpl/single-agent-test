using System.Text.Json.Serialization;

namespace WazuhCommon.Models
{
    public class CommandRequest : RequestBase
    {
        [JsonPropertyName("command")]
        public string Command { get; set; }

        [JsonPropertyName("arguments")]
        public List<string> Arguments { get; set; }

        [JsonPropertyName("custom")]
        public bool Custom { get; set; }

        [JsonPropertyName("alert")]
        public Alert Alert { get; set; }
    }
}
