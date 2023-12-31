using System.Text;
using System.Text.Json.Serialization;

namespace WazuhCommon.Models
{
    public class AgentListResponse : ResponseBase
    {
        [JsonPropertyName("affected_items")]
        public List<EndPoint> AffectedItems { get; set; }

        [JsonPropertyName("total_affected_items")]
        public int TotalAffectedItems { get; set; }

        [JsonPropertyName("total_failed_items")]
        public int TotalFailedItems { get; set; }

        [JsonPropertyName("failed_items")]
        public List<EndPoint> FailedItems { get; set; }

        public AgentListResponse()
        {
            AffectedItems = [];
            FailedItems = [];
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Total Affected Items: {TotalAffectedItems}");
            builder.AppendLine($"Affected Items: {string.Join(",", AffectedItems.Select(x => x.Id))}"); 
            builder.AppendLine($"Total Failed Items: {TotalFailedItems}");
            builder.Append($"Failed Items: {string.Join(",", FailedItems.Select(x => x.Id))}");
            return builder.ToString();
        }
    }
}
