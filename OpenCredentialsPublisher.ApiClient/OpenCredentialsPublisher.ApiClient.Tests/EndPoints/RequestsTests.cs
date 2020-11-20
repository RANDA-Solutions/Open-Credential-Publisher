using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCredentialsPublisher.ApiClient.EndPoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialsPublisher.ApiClient.EndPoints.Tests
{
    [TestClass()]
    public class RequestsTests
    {
        [TestMethod()]
        public void GetRequestTest() {
            var pub = ApiClient.Tests.ApiTestHelper.GetPublish();

            var req = Requests.GetRequest(pub.RequestId, ApiClient.Tests.ApiTestHelper.GetToken().AccessToken).Result;

            int countdown = 12;
            while (req.Status == "Queued" && countdown > 0) {
                System.Threading.Thread.Sleep(5000);
                req = Requests.GetRequest(pub.RequestId, ApiClient.Tests.ApiTestHelper.GetToken().AccessToken).Result;
                countdown--;
            }

            Assert.IsTrue(countdown > 0);
        }
    }
}