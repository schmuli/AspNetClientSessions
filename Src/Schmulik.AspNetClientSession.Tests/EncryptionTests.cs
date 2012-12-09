using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Schmulik.AspNetClientSession.Cryptography;

namespace Schmulik.AspNetClientSession.Tests
{
    [TestClass]
    public class EncryptionTests
    {
        [TestMethod]
        public void Encryption_RandomSecretKeysAreUnique()
        {
            var key = RandomSecretKey.Generate();
            var key2 = RandomSecretKey.Generate();

            Assert.AreNotEqual(key, key2);
        }

        [TestMethod]
        public void Encryption_EncryptDecrypt()
        {
            const string secret = "a secret key";

            var encryptionKey = Hmac.Digest(secret, "clientsession-encryption");
            var signatureKey = Hmac.Digest(secret, "clientsession-signature");

            var encryption = new Encryption(encryptionKey, signatureKey);

            const string content = "some plain text content";
            var encrypted = encryption.Encrypt(content);
            var result = encryption.Decrypt(encrypted);

            Assert.AreEqual(content, result);
        }

        [TestMethod]
        public void Encryption_IgnoreTamperedValues()
        {
            const string secret = "a secret key";

            var encryptionKey = Hmac.Digest(secret, "clientsession-encryption");
            var signatureKey = Hmac.Digest(secret, "clientsession-signature");

            var encryption = new Encryption(encryptionKey, signatureKey);

            const string content = "some plain text content";
            var encrypted = encryption.Encrypt(content);
            var result2 = encryption.Decrypt(encrypted.Replace(encrypted[4], 'r'));

            Assert.IsNull(result2);
        }

        [TestMethod]
        public void Encryption_EncodeBase64UrlString()
        {
            const string secret = "a secret key";
            var encryptionKey = Hmac.Digest(secret, "clientsession-encryption");

            const string content = "some string content";

            var encrypted = Aes.Encrypt(content, encryptionKey);

            encrypted = Base64Url.Encode(encrypted);

            StringAssert.Matches(encrypted, new Regex("[^/+=]"));
        }

        [TestMethod]
        public void Encryption_DecodeBase64UrlString()
        {
            const string secret = "a secret key";
            var encryptionKey = new Hmac(secret).Update("clientsession-encryption").Digest();

            const string content = "some string content y";

            var encrypted = Aes.Encrypt(content, encryptionKey);
            var encryptedSafe = Base64Url.Encode(encrypted);
            var decrypted = Base64Url.Decode(encryptedSafe);

            Assert.AreEqual(decrypted, encrypted);
        }
    }
}
