using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PemUtils;
using OpenCredentialsPublisher.Credentials.Clrs.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using IdentityModel;
using OpenCredentialsPublisher.Credentials.Cryptography;
using OpenCredentialsPublisher.Credentials.Clrs.Keys;

namespace OpenCredentialsPublisher.Credentials.Clrs.Utilities
{
    /// <summary>
    /// Based upon IMS Global's CLR reference implementation for managing Signing Credentials
    /// </summary>
    public class CryptoUtility
    {
        private readonly IKeyStorage _keyStorage;

        public CryptoUtility(IKeyStorage keyStorage)
        {
            _keyStorage = keyStorage;
        }

        /// <summary>
        /// Get the signing credentials for an issuer.
        /// </summary>
        /// <param name="issuerId">The issuer id.</param>
        /// <returns>A Tuple including the signing credential and the issuer id.</returns>
        public (SigningCredentials credentials, string issuerId) GetSigningCredentialsByIssuerId(string issuerId)
        {
            return GetSigningCredentials(issuerId: issuerId);
        }

        /// <summary>
        /// Get the signing credentials for a key.
        /// </summary>
        /// <param name="keyId">The key id.</param>
        /// <returns>A Tuple including the signing credential and the issuer id.</returns>
        public (SigningCredentials credentials, string issuerId) GetSigningCredentialsByKeyId(string keyId)
        {
            return GetSigningCredentials(keyId: keyId);
        }

        /// <summary>
        /// Get the signing credentials for an issuer or key id.
        /// </summary>
        private (SigningCredentials credentials, string issuerId) GetSigningCredentials(string issuerId = null, string keyId = null)
        {
            RsaKeySet rsaKeySet = _keyStorage.GetKeySet();

            rsaKeySet ??= new RsaKeySet
            {
                Keys = new List<RsaKey>()
            };

            var rsaKey = issuerId == null
                ? rsaKeySet.Keys.SingleOrDefault(x => x.KeyId == keyId)
                : rsaKeySet.Keys.SingleOrDefault(x => x.IssuerId == issuerId);

            if (rsaKey == null && issuerId == null)
            {
                // Only create a key when signing

                return (null, null);
            }

            if (rsaKey == null)
            {
                var key = CreateRsaSecurityKey();

                var parameters = key.Rsa?.ExportParameters(includePrivateParameters: true) ?? key.Parameters;

                rsaKey = new RsaKey
                {
                    Parameters = parameters,
                    KeyId = key.KeyId,
                    IssuerId = issuerId
                };

                rsaKeySet.Keys.Add(rsaKey);
                _keyStorage.UpdateKeySet(rsaKeySet);
            }

            var securityKey = new RsaSecurityKey(rsaKey.Parameters)
            {
                KeyId = rsaKey.KeyId
            };

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            return (signingCredentials, rsaKey.IssuerId);
        }

        /// <summary>
        /// </summary>
        private static RsaSecurityKey CreateRsaSecurityKey()
        {
            var rsaBlob = CryptoMethods.GenerateRsaKey();
            var parameters = CryptoMethods.FromCspBlobToRSAParameters(rsaBlob);
            var key = new RsaSecurityKey(parameters) { KeyId = CryptoRandom.CreateUniqueId(16) };
            return key;
        }

        /// <summary>
        /// Get the public key PEM for the signing credentials. Returns null
        /// if the key is not an <see cref="RsaSecurityKey"/>.
        /// </summary>
        public static string GetPublicKey(SigningCredentials credentials)
        {
            try
            {
                var key = (RsaSecurityKey)credentials.Key;

                using var stream = new MemoryStream();
                using var writer = new PemWriter(stream);
                writer.WritePublicKey(key.Parameters);
                stream.Position = 0;

                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
