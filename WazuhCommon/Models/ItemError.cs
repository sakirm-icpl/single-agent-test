using System.Text.Json.Serialization;

namespace WazuhCommon.Models
{
    public class ItemError
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return $"Code: {Code}, Message: {Message}";
        }
    }
}
