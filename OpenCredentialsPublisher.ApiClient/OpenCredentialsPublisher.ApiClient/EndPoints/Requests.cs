using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenCredentialsPublisher.ApiClient.EndPoints
{
    public class Requests
    {
        #region Statics
        public static async Task<Requests> GetRequest(string RequestId, string BearerToken) {
            return await EndPointBase.ConnectGet<Requests>("api/requests", $"/{RequestId}", BearerToken);
        }
        #endregion

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("accessKey")]
        public string AccessKey { get; set; }

        [JsonProperty("qrCode")]
        public QrCodeType QrCode { get; set; }

        public class QrCodeType
        {
            [JsonProperty("mimeType")]
            public string MimeType { get; set; }

            [JsonProperty("data")]
            public string Data { get; set; }
        }
    }
}
