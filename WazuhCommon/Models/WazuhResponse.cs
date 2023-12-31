using System.Text.Json.Serialization;

namespace WazuhCommon.Models
{
    public class WazuhResponse<T> where T : ResponseBase
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("error")]
        public int ErrorCode { get; set; }

        public override string ToString()
        {
            return $"Data: {Data}, Message: {Message}, ErrorCode: {ErrorCode}";
        }   
    }
}
