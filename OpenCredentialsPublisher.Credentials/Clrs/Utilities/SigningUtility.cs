using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenCredentialsPublisher.Credentials.Clrs.Clr;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace OpenCredentialsPublisher.Credentials.Clrs.Utilities
{
    public class SigningUtility
    {
        private readonly CryptoUtility _cryptoUtility;
        public SigningUtility(CryptoUtility cryptoUtility)
        {
            _cryptoUtility = cryptoUtility;
        }

        public string Sign(ClrDType clr, Uri baseUri, bool removeAfterSigned = true)
        {
            if (clr != null)
            {
                if (clr.Learner.Endorsements != null)
                {
                    foreach (var endorsement in clr.Learner.Endorsements.Where(e => e.Verification?.Type == VerificationTypeEnum.Signed).ToList())
                    {
                        var signedEndorsement = SignEndorsement(endorsement, baseUri);
                        clr.Learner.SignedEndorsements ??= new List<string>();
                        clr.Learner.SignedEndorsements.Add(signedEndorsement);
                        if (removeAfterSigned)
                            clr.Learner.Endorsements.Remove(endorsement);
                    }
                }

                if (clr.Publisher.Endorsements != null)
                {
                    foreach (var endorsement in clr.Publisher.Endorsements.Where(e => e.Verification?.Type == VerificationTypeEnum.Signed).ToList())
                    {
                        var signedEndorsement = SignEndorsement(endorsement, baseUri);
                        clr.Publisher.SignedEndorsements ??= new List<string>();
                        clr.Publisher.SignedEndorsements.Add(signedEndorsement);
                        if (removeAfterSigned)
                            clr.Publisher.Endorsements.Remove(endorsement);
                    }
                }

                var signedAssertions = new List<string>();
                foreach (var assertion in clr.Assertions)
                {
                    if (assertion.Endorsements != null)
                    {
                        foreach (var endorsement in assertion.Endorsements.Where(e => e.Verification?.Type == VerificationTypeEnum.Signed).ToList())
                        {
                            var signedEndorsement =
                                SignEndorsement(endorsement, baseUri);
                            assertion.SignedEndorsements ??= new List<string>();
                            assertion.SignedEndorsements.Add(signedEndorsement);
                            if (removeAfterSigned)
                                assertion.Endorsements.Remove(endorsement);
                        }
                    }

                    if (assertion.Achievement?.Endorsements != null)
                    {
                        foreach (var endorsement in assertion.Achievement.Endorsements.Where(e => e.Verification?.Type == VerificationTypeEnum.Signed).ToList())
                        {
                            var signedEndorsement =
                                SignEndorsement(endorsement, baseUri);
                            assertion.Achievement.SignedEndorsements ??= new List<string>();
                            assertion.Achievement.SignedEndorsements.Add(signedEndorsement);
                            if (removeAfterSigned)
                                assertion.Achievement.Endorsements.Remove(endorsement);
                        }
                    }

                    if (assertion.Achievement?.Issuer.Endorsements != null)
                    {
                        foreach (var endorsement in
                            assertion.Achievement.Issuer.Endorsements.Where(e => e.Verification?.Type == VerificationTypeEnum.Signed).ToList())
                        {
                            var signedEndorsement =
                                SignEndorsement(endorsement, baseUri);
                            assertion.Achievement.Issuer.SignedEndorsements ??= new List<string>();
                            assertion.Achievement.Issuer.SignedEndorsements.Add(
                                signedEndorsement);
                            if (removeAfterSigned)
                                assertion.Achievement.Issuer.Endorsements.Remove(endorsement);
                        }
                    }

                    if (assertion.Verification?.Type == VerificationTypeEnum.Signed)
                    {
                        var signedAssertion = SignAssertion(assertion, baseUri);
                        clr.SignedAssertions ??= new List<string>();
                        clr.SignedAssertions.Add(signedAssertion);
                        signedAssertions.Add(assertion.Id);
                    }
                }

                if (removeAfterSigned)
                    clr.Assertions.RemoveAll(a => signedAssertions.Contains(a.Id));
                return SignClr(clr, baseUri);
            }
            return null;
        }

        /// <summary>
        /// Return the public key.
        /// </summary>
        /// <param name="keysApiUrl">The keys API URL.</param>
        /// <param name="keyId">The key id.</param>
        public CryptographicKeyDType GetCryptographicKey(string keysApiUrl, string keyId)
        {
            var (credentials, issuerId) = _cryptoUtility.GetSigningCredentialsByKeyId(keyId);

            return GetCryptographicKey(keysApiUrl, issuerId, credentials);
        }

        /// <summary>
        /// Return the public key.
        /// </summary>
        /// <param name="keysApiUrl">The keys API URL.</param>
        /// <param name="keyId">The key id.</param>
        public CryptographicKeyDType GetCryptographicKey(string keysApiUrl, string issuerId, SigningCredentials credentials)
        {
            if (credentials == null)
            {
                return default;
            }

            return new CryptographicKeyDType
            {
                Id = $"{keysApiUrl}/{credentials.Key.KeyId}",
                Owner = issuerId,
                PublicKeyPem = CryptoUtility.GetPublicKey(credentials)
            };
        }

        /// <summary>
        /// Get the revocation list for an issuer. The issuer is found using the issuer's key id.
        /// </summary>
        /// <param name="revocationListApiUrl"></param>
        /// <param name="keyId"></param>
        public RevocationListDType GetRevocationList(string revocationListApiUrl, string keyId)
        {
            var (credentials, issuerId) = _cryptoUtility.GetSigningCredentialsByKeyId(keyId);

            if (credentials == null)
            {
                return default;
            }

            return new RevocationListDType
            {
                Id = $"{revocationListApiUrl}/{keyId}",
                Issuer = issuerId,
                RevokedAssertions = new List<string>()
            };
        }

        /// <summary>
        /// Return a signed (JWS) clr
        /// </summary>
        private string SignClr(ClrDType clr, Uri baseUri)
        {
            var handler = new JwtSecurityTokenHandler();
            var (credentials, _) = _cryptoUtility.GetSigningCredentialsByIssuerId(clr.Publisher.Id);

            var cryptographicKey = GetCryptographicKey($"{baseUri}/keys", clr.Publisher.Id, credentials);

            var issuer = clr.Publisher;
            cryptographicKey.Owner = issuer.Id;
            issuer.PublicKey = cryptographicKey;
            issuer.RevocationList = $"{baseUri}/revocations/{credentials.Key.KeyId}";

            clr.Verification = new VerificationDType
            {
                Type = VerificationTypeEnum.Signed
            };

            var assertionJson = JsonConvert.SerializeObject(clr);
            var payload = JwtPayload.Deserialize(assertionJson);

            var token = new JwtSecurityToken(new JwtHeader(credentials), payload);
            
            return handler.WriteToken(token);
        }

        /// <summary>
        /// Return a signed (JWS) assertion
        /// </summary>
        private string SignAssertion(AssertionDType assertion, Uri baseUri)
        {
            var handler = new JwtSecurityTokenHandler();
            var (credentials, _) = _cryptoUtility.GetSigningCredentialsByIssuerId(assertion.Achievement.Issuer.Id);

            var cryptographicKey = GetCryptographicKey($"{baseUri}/keys", assertion.Achievement.Issuer.Id, credentials);

            var issuer = assertion.Achievement.Issuer;
            cryptographicKey.Owner = issuer.Id;
            issuer.PublicKey = cryptographicKey;
            issuer.RevocationList = $"{baseUri}/revocations/{credentials.Key.KeyId}";

            assertion.Verification = new VerificationDType
            {
                Type = VerificationTypeEnum.Signed
            };

            var assertionJson = JsonConvert.SerializeObject(assertion);
            var payload = JwtPayload.Deserialize(assertionJson);

            var token = new JwtSecurityToken(new JwtHeader(credentials), payload);
            
            return handler.WriteToken(token);
        }

        private string SignEndorsement(EndorsementDType endorsement, Uri baseUri)
        {
            var handler = new JwtSecurityTokenHandler();
            var (credentials, _) = _cryptoUtility.GetSigningCredentialsByIssuerId(endorsement.Issuer.Id);

            var cryptographicKey = GetCryptographicKey($"{baseUri}/keys", endorsement.Issuer.Id, credentials);

            var issuer = endorsement.Issuer;
            cryptographicKey.Owner = issuer.Id;
            issuer.PublicKey = cryptographicKey;
            issuer.RevocationList = $"{baseUri}/revocations/{credentials.Key.KeyId}";

            endorsement.Verification = new VerificationDType
            {
                Type = VerificationTypeEnum.Signed
            };

            var endorsementJson = JsonConvert.SerializeObject(endorsement);
            var payload = JwtPayload.Deserialize(endorsementJson);

            var token = new JwtSecurityToken(new JwtHeader(credentials), payload);

            return handler.WriteToken(token);
        }
    }
}
