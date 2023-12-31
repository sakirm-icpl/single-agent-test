using System.Text.Json.Serialization;

namespace WazuhCommon.Models
{
    public class Alert
    {
        [JsonPropertyName("data")]
        public Dictionary<string, object> Data { get; set; }
    
        public Alert()
        {
            Data = new Dictionary<string, object>();
        }
    }
}
