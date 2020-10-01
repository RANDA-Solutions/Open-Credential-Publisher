using OpenCredentialsPublisher.Credentials.Clrs.Keys;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialsPublisher.Credentials.Clrs.Interfaces
{
    public interface IKeyStorage
    {
        RsaKeySet GetKeySet();
        void UpdateKeySet(RsaKeySet keySet);
    }
}
