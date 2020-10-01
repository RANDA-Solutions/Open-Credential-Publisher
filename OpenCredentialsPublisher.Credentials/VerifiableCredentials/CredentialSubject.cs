using Newtonsoft.Json;
using System;

namespace OpenCredentialsPublisher.Credentials.VerifiableCredentials
{

    public interface ICredentialSubject
    {
        [JsonProperty("id", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        String Id { get; set; }
    }
    public abstract class CredentialSubject: ICredentialSubject
    {
        public String Id { get; set; }
    }
}
