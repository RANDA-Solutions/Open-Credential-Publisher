using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OpenCredentialsPublisher.Credentials.VerifiableCredentials
{

    public class VerifiableCredential
    {
        [JsonProperty("@context", Order = 1)]
        public List<String> Contexts { get; set; }

        [JsonProperty("id", Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        public String Id { get; set; }

        [JsonProperty("type", Order = 3)]
        public List<String> Types { get; set; }

        [JsonProperty("issuer", Order = 4)]
        public String Issuer { get; set; }

        [JsonProperty("issuanceDate", Order = 5)]
        [JsonConverter(typeof(DateConverter<DateTime>), "o")]
        public DateTime IssuanceDate { get; set; }

        [JsonProperty("credentialSubject", Order = 6)]
        [JsonConverter(typeof(SingleOrArrayConverter<ICredentialSubject>))]
        public List<ICredentialSubject> CredentialSubjects { get; set; }

        [JsonProperty("credentialStatus", Order = 7, NullValueHandling = NullValueHandling.Ignore)]
        public CredentialStatus CredentialStatus { get; set; }

        

        public void SetIssuer(Uri uri)
        {
            Issuer = uri.ToString();
        }

        public void SetIssuer(String did, String name)
        {
            var issuer = new Issuer
            {
                Id = did,
                Name = name
            };

            Issuer = JsonConvert.SerializeObject(issuer);
        }

        public void SetIssuer(Uri uri, String name)
        {
            var issuer = new Issuer
            {
                Id = uri.ToString(),
                Name = name
            };

            Issuer = JsonConvert.SerializeObject(issuer);
        }
         
    }
}
