using System.Text;
using System.Text.Json.Serialization;

namespace WazuhCommon.Models
{
    public class OperatingSystem
    {
        [JsonPropertyName("build")]
        public string Build { get; set; }

        [JsonPropertyName("major")]
        public string Major { get; set; }

        [JsonPropertyName("minor")]
        public string Minor { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("platform")]
        public string Platform { get; set; }

        [JsonPropertyName("uname")]
        public string Uname { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Name: {Name}");
            builder.AppendLine($"Version: {Version}");
            builder.AppendLine($"Build: {Build}");
            // Add other properties as needed
            return builder.ToString();
        }
    }

}
