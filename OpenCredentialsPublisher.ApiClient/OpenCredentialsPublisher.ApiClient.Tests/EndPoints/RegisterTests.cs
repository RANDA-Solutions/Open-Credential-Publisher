using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCredentialsPublisher.ApiClient.EndPoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialsPublisher.ApiClient.EndPoints.Tests
{
    [TestClass()]
    public class RegisterTests
    {
        [TestMethod()]
        public void GetRegisterTest() {
            string clientName = "ocp api client";
            string clientUri = "https://localhost/ocpclient";

            var r = Register.RegisterClient(clientName, clientUri).Result;

            Assert.IsNotNull(r);
        }
    }
}