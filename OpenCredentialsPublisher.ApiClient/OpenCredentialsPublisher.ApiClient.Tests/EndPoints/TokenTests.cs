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
            string clientName = "ocp api client";
            string clientUri = "https://localhost/ocpclient";

            var r = Register.GetRegister(clientName, clientUri).Result;

            var t = Token.GetToken(r).Result;

            Assert.IsNotNull(t);
            Assert.IsTrue(t.AccessToken.Length > 0);
        }
    }
}