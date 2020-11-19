using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OpenCredentialsPublisher.ApiClient.Models.Request
{
    class PublishVM
    {
        [JsonProperty("identity")]
        public string Identity { get; set; }

        [JsonProperty("clr")]
        public object CLR { get; set; }
    }
}
