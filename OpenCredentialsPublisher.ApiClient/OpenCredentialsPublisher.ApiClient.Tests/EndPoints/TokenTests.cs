using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCredentialsPublisher.ApiClient.EndPoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialsPublisher.ApiClient.EndPoints.Tests
{
    [TestClass()]
    public class TokenTests
    {
        [TestMethod()]
        public void GetTokenTest() {
            var t = ApiClient.Tests.ApiTestHelper.GetToken();

            Assert.IsNotNull(t);
            Assert.IsTrue(t.AccessToken.Length > 0);
        }
    }
}