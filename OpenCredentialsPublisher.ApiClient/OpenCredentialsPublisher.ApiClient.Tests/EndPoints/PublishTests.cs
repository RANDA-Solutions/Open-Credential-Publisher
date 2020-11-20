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

            var t = ApiClient.Tests.ApiHelper.GetToken();

            string identity = Guid.NewGuid().ToString();

            var result = Publish.PublishClr(t.AccessToken, identity, jsonClr).Result;

            Assert.IsFalse(String.IsNullOrEmpty(result.RequestId));
            Assert.IsFalse(result.Error);
            Assert.IsTrue(String.IsNullOrEmpty(result.ErrorMessage));
        }
    }
}