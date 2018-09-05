using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;

namespace Homedish.Core.Cryptography
{
    public class Encryptor : IEncryptor
    {
        private readonly IDataProtector _protector;

        public Encryptor(IDataProtectionProvider provider, IConfiguration configuration)
        {
            const string featureNameKey = "FeatureName";

            var featureName = configuration[featureNameKey];

            if (string.IsNullOrWhiteSpace(featureName))
            {
                throw new InvalidOperationException($"The {featureNameKey} key should be specified in the configs");
            }

            _protector = provider.CreateProtector(featureName);
        }

        public string Encrypt(string plaintext)
        {
            return _protector.Protect(plaintext);
        }

        public string Decrypt(string encryptedText)
        {
            return _protector.Unprotect(encryptedText);
        }
    }
}
