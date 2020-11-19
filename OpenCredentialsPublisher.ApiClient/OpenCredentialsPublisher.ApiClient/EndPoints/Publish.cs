using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialsPublisher.ApiClient.EndPoints
{
    public class Publish
    {
        #region Statics
        public static async Task<Publish> PublishClr(string BearerToken, string Identity, string JsonClr) {
            var reqData = new Models.Request.PublishVM() {
                Identity = Identity,
                CLR = Newtonsoft.Json.JsonConvert.DeserializeObject(JsonClr)
            };
            return await EndPointBase.ConnectJson<Publish>("api/publish", reqData, BearerToken);
        }
        #endregion
    }
}