using System;
using System.Collections.Generic;
using System.Text;
using OpenCredentialsPublisher.ApiClient.EndPoints;

namespace OpenCredentialsPublisher.ApiClient.Tests
{
    static class ApiHelper
    {
        static Register RegisterData { get; set; }

        static Token TokenData { get; set; }

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
    }
}