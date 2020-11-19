using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCredentialsPublisher.ApiClient.EndPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCredentialsPublisher.ApiClient.EndPoints.Tests
{
    [TestClass()]
    public class PublishTests
    {
        [TestMethod()]
        public void PublishClrTest() {
            string filename = typeof(PublishTests).Assembly.GetManifestResourceNames().Single(s => s.EndsWith("Files.sample clr.json"));
            string jsonClr;
            using (var sr = new System.IO.StreamReader(typeof(PublishTests).Assembly.GetManifestResourceStream(filename))) {
                jsonClr = sr.ReadToEnd();
            }

            string clientName = "ocp api client";
            string clientUri = "https://localhost/ocpclient";

            var r = Register.RegisterClient(clientName, clientUri).Result;
            var t = Token.GetBearerToken(r).Result;

            var result = Publish.PublishClr(t.AccessToken, jsonClr).Result;
        }
    }
}