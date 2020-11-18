using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OpenCredentialsPublisher.ApiClient.Models.Request
{
    class RegisterVM
    {
        [JsonProperty("client_name")]
        public string ClientName { get; set; }

        [JsonProperty("client_uri")]
        public string ClientUri { get; set; }

        [JsonProperty("token_endpoint_auth_method")]
        public string TokenEndpointAuthMethod { get; set; } = "client_secret_basic";

        [JsonProperty("scope")]
        public string Scope { get; set; } = "ocp-publisher";
    }
}
