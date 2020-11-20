using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OpenCredentialsPublisher.ApiClient.Models.Request
{
    class PublishVM
    {
        [JsonProperty("identity")]
        public IdentityType Identity { get; set; }

        [JsonProperty("clr")]
        public object CLR { get; set; }

        public class IdentityType
        {
            [JsonProperty("id")]
            public string ID { get; set; }
        }
    }
}