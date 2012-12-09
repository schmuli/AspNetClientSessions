namespace Schmulik.AspNetClientSession.Cryptography
{
    /// <summary>
    /// Encrypts, decrypts, signs and validates encrypted data.
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// Key used to encrypt data.
        /// </summary>
        private readonly string _encryptionKey;

        /// <summary>
        /// Key used to sign encrypted data.
        /// </summary>
        private readonly string _signatureKey;

        public Encryption(string encryptionKey, string signatureKey)
        {
            _encryptionKey = encryptionKey;
            _signatureKey = signatureKey;
        }

        /// <summary>
        /// Encrypts and hashes plain text, combining the result into a 
        /// URL safe Base64 string.
        /// </summary>
        /// <param name="plaintext">The text to encrypt</param>
        /// <returns>A URL safe Base64 string</returns>
        public string Encrypt(string plaintext)
        {
            var ciphertext = Aes.Encrypt(plaintext, _encryptionKey);

            var hash = ComputeHash(ciphertext);

            return Base64Url.Encode(ciphertext) + "." + Base64Url.Encode(hash);
        }

        /// <summary>
        /// Validates and decrypts encrypted data.
        /// </summary>
        /// <param name="encrypted">URL safe Base64 encrypted text</param>
        /// <returns>Decrypted text if validated, otherwise null</returns>
        public string Decrypt(string encrypted)
        {
            var components = encrypted.Split('.');
            if (components.Length != 2)
            {
                return null;
            }

            var ciphertext = Base64Url.Decode(components[0]);
            var hash = Base64Url.Decode(components[1]);

            var expectedHash = ComputeHash(ciphertext);
            if (hash != expectedHash)
            {
                return null;
            }

            var decrypted = Aes.Decrypt(ciphertext, _encryptionKey);
            return decrypted;
        }

        /// <summary>
        /// Computes a HMAC hash for the specified encrypted value.
        /// </summary>
        /// <param name="ciphertext">The encrypted value</param>
        /// <returns>A HMAC hash</returns>
        private string ComputeHash(string ciphertext)
        {
            return Hmac.Digest(_signatureKey, ciphertext);
        }
    }
}
