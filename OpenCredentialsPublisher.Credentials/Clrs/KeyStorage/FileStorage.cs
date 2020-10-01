using Newtonsoft.Json;
using OpenCredentialsPublisher.Credentials.Clrs.Interfaces;
using OpenCredentialsPublisher.Credentials.Clrs.Keys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenCredentialsPublisher.Credentials.Clrs.KeyStorage
{
    /// <summary>
    /// Sample implementation based upon IMS Global's CLR Reference Project
    /// https://github.com/IMSGlobal/CLR-reference-implementation/blob/develop/ClrProvider/src/Crypto.cs
    /// </summary>
    public class FileStorage : IKeyStorage
    {
        private const string KeyFileName = "keys.rsa";
        public RsaKeySet GetKeySet()
        {
            var filename = Path.Combine(Directory.GetCurrentDirectory(), KeyFileName);
            RsaKeySet rsaKeySet = null;
            if (File.Exists(filename))
            {
                var keysFile = File.ReadAllText(filename);
                rsaKeySet = JsonConvert.DeserializeObject<RsaKeySet>(keysFile, new JsonSerializerSettings
                {
                    ContractResolver = new RsaKeyContractResolver()
                });
            }
            return rsaKeySet;
        }

        public void UpdateKeySet(RsaKeySet rsaKeySet)
        {
            var filename = Path.Combine(Directory.GetCurrentDirectory(), KeyFileName);
            File.WriteAllText(filename, JsonConvert.SerializeObject(rsaKeySet, new JsonSerializerSettings
            {
                ContractResolver = new RsaKeyContractResolver()
            }));
        }
    }
}
