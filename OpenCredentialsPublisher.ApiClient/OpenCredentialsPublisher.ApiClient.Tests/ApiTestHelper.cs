using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCredentialsPublisher.ApiClient.EndPoints;

namespace OpenCredentialsPublisher.ApiClient.Tests
{
    /// <summary>
    /// Test wrapper class to prevent overloading API endpoints through multiple test runs
    /// </summary>
    static class ApiTestHelper
    {
        static Register RegisterData { get; set; }

        static Token TokenData { get; set; }

        static Publish PublishData { get; set; }

        public static Register GetRegistration() {
            string clientName = "ocp api client";
            string clientUri = "https://localhost/ocpclient";

            if (RegisterData == null) {
                RegisterData = Register.RegisterClient(clientName, clientUri).Result;
            }

            return RegisterData;
        }

        public static Token GetToken() {
            if (TokenData == null) {
                TokenData = Token.GetBearerToken(GetRegistration()).Result;
            }
            return TokenData;
        }

        public static Publish GetPublish() {
            string filename = typeof(ApiTestHelper).Assembly.GetManifestResourceNames().Single(s => s.EndsWith("Files.sample clr.json"));
            string jsonClr;
            using (var sr = new System.IO.StreamReader(typeof(ApiTestHelper).Assembly.GetManifestResourceStream(filename))) {
                jsonClr = sr.ReadToEnd();
            }

            var t = GetToken();

            string identity = Guid.NewGuid().ToString();

            return Publish.PublishClr(t.AccessToken, identity, jsonClr).Result;
        }
    }
}