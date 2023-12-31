using System.Text.Json.Serialization;

namespace WazuhCommon.Models
{
    public class FailedItem
    {
        [JsonPropertyName("error")]
        public ItemError ItemError { get; set; }

        [JsonPropertyName("id")]
        public List<string> Id { get; set; }

        public override string ToString()
        {
            return $"Error: {ItemError}, Id: {string.Join(",", Id)}";
        }
    }
}
