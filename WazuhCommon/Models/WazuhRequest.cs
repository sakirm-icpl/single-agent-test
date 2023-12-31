using System.Text.Json.Serialization;

namespace WazuhCommon.Models
{
    public class WazuhRequest<T> where T : RequestBase
    {
        [JsonPropertyName("method")]
        public HttpMethod Method { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("body")]
        public T Body { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        public override string ToString()
        {
            return $"Method: {Method}, Path: {Path}, Body: {Body}, Id: {Id}";
        }
    }
}
