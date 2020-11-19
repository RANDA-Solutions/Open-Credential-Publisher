using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenCredentialsPublisher.ApiClient.EndPoints
{
    public class Register
    {
        #region Statics
        public static async Task<Register> RegisterClient(string ClientName, string ClientUri) {
            Models.Request.RegisterVM reqData = new Models.Request.RegisterVM() {
                ClientName = ClientName,
                ClientUri = ClientUri
            };

            return await EndPointBase.ConnectJson<Register>("connect/register", reqData);
        }
        #endregion

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("client_id_issued_at")]
        public int ClientIdIssuedAt { get; set; }

        [JsonProperty("client_secret_expires_at")]
        public int ClientSecretExpiresAt { get; set; }

        [JsonProperty("client_name")]
        public string ClientName { get; set; }

        [JsonProperty("client_uri")]
        public string ClientUri { get; set; }

        [JsonProperty("token_endpoint_auth_method")]
        public string TokenEndPointAuthMethod { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("grant_types")]
        public string[] GrantTypes { get; set; }
    }
}
