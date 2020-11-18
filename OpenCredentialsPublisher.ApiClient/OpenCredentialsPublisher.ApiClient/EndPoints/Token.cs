using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenCredentialsPublisher.ApiClient.EndPoints
{
    public class Token
    {
        #region Statics
        public static async Task<Token> GetToken(Register RegisterResponse) {
            Dictionary<string, string> data = new Dictionary<string, string>() {
                { "grant_type", "client_credentials" },
                { "scope", "ocp-publisher" },
                { "client_id", RegisterResponse.ClientId },
                { "client_secret", RegisterResponse.ClientSecret }
            };

            return await EndPointBase.ConnectForm<Token>("connect/token", data);
        }
        #endregion

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}