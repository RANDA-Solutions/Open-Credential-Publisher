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
            var result = ApiClient.Tests.ApiTestHelper.GetPublish();

            Assert.IsFalse(String.IsNullOrEmpty(result.RequestId));
            Assert.IsFalse(result.Error);
            Assert.IsTrue(String.IsNullOrEmpty(result.ErrorMessage));
        }
    }
}