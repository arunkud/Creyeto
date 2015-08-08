using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ecom.presentation.website.Models
{
    public class SearchInfo
    {
        [JsonProperty(PropertyName = "value")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "data")]
        public string ID { get; set; }
    }
}