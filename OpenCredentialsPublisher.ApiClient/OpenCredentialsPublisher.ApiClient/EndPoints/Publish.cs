using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialsPublisher.ApiClient.EndPoints
{
    public class Publish
    {
        #region Statics
        public static async Task<Publish> PublishClr(string BearerToken, string JsonClr) => await EndPointBase.ConnectJson<Publish>("api/publish", JsonClr, BearerToken);
        #endregion
    }
}