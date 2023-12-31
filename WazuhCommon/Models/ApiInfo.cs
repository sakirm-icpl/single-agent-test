using System.Text.Json.Serialization;

namespace WazuhCommon.Models
{
    public class ApiInfo : ResponseBase
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; }

        [JsonPropertyName("revision")]
        public int Revision { get; set; }

        [JsonPropertyName("license_name")]
        public string LicenseName { get; set; }

        [JsonPropertyName("license_url")]
        public string LicenseUrl { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return $"Title: {Title}, ApiVersion: {ApiVersion}, Revision: {Revision}, LicenseName: {LicenseName}, LicenseUrl: {LicenseUrl}, Hostname: {Hostname}, Timestamp: {Timestamp}";
        }
    }
}
