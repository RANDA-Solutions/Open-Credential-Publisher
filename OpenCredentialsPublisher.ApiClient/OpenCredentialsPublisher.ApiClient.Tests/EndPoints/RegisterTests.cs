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
            var r = ApiClient.Tests.ApiHelper.GetRegistration();

            Assert.IsNotNull(r);
        }
    }
}