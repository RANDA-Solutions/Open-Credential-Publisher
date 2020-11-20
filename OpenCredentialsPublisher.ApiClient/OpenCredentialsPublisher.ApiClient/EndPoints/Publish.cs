using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenCredentialsPublisher.ApiClient.EndPoints
{
    public class Publish
    {
        #region Statics
        public static async Task<Publish> PublishClr(string BearerToken, string Identity, string JsonClr) {
            var reqData = new Models.Request.PublishVM() {
                Identity = new Models.Request.PublishVM.IdentityType() {
                    ID = Identity
                },
                CLR = JsonConvert.DeserializeObject(JsonClr)
            };
            return await EndPointBase.ConnectJson<Publish>("api/publish", reqData, BearerToken);
        }
        #endregion

        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}