using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecom.presentation.website.Models
{
    public class DoctorSearchResponseInfo
    {
        [JsonProperty(PropertyName ="n")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "src")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "q")]
        public string Qualification { get; set; }

        [JsonProperty(PropertyName = "s")]
        public string Specialization { get; set; }

        [JsonProperty(PropertyName = "h")]
        public string Hospital { get; set; }

        [JsonProperty(PropertyName = "ll")]
        public string[] Location { get; set; }

        [JsonProperty(PropertyName = "f")]
        public decimal? Fee { get; set; }

        [JsonProperty(PropertyName = "c")]
        public string ContactNumber { get; set; }

        [JsonProperty(PropertyName = "lc")]
        public int LikeCount { get; set; }

        [JsonProperty(PropertyName = "rc")]
        public int ReviewCount { get; set; }

        [JsonProperty(PropertyName = "ad")]
        public string Address { get; set; }
    }
}