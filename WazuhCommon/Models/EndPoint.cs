using System.Text;
using System.Text.Json.Serialization;

namespace WazuhCommon.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class EndPoint
    {
        [JsonPropertyName("os")]
        public OperatingSystem OperatingSystem { get; set; }

        [JsonPropertyName("registerIP")]
        public string RegisterIP { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("disconnection_time")]
        public DateTime DisconnectionTime { get; set; }

        [JsonPropertyName("group_config_status")]
        public string GroupConfigStatus { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("ip")]
        public string IpAddress { get; set; }

        [JsonPropertyName("dateAdd")]
        public DateTime DateAdd { get; set; }

        [JsonPropertyName("node_name")]
        public string NodeName { get; set; }

        [JsonPropertyName("lastKeepAlive")]
        public DateTime LastKeepAlive { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("manager")]
        public string Manager { get; set; }

        [JsonPropertyName("configSum")]
        public string ConfigSum { get; set; }

        [JsonPropertyName("group")]
        public List<string> Group { get; set; }

        [JsonPropertyName("mergedSum")]
        public string MergedSum { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Name: {Name}");
            builder.AppendLine($"ID: {Id}");
            builder.AppendLine($"Status: {Status}");
            builder.AppendLine($"OS: {OperatingSystem}");
            builder.AppendLine($"IP Address: {IpAddress}");
            builder.AppendLine($"Register IP: {RegisterIP}");
            builder.AppendLine($"Version: {Version}");
            builder.AppendLine($"Date Added: {DateAdd}");
            builder.AppendLine($"Last Keep Alive: {LastKeepAlive}");
            builder.AppendLine($"Node Name: {NodeName}");
            builder.AppendLine($"Manager: {Manager}");
            builder.AppendLine($"Group Config Status: {GroupConfigStatus}");
            builder.AppendLine($"Config Sum: {ConfigSum}");
            builder.AppendLine($"Merged Sum: {MergedSum}");
            builder.Append($"Groups: {string.Join(",", Group)}");            
            return builder.ToString();
        }
    }
}
