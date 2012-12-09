using System;
using System.Security.Cryptography;
using System.Text;

namespace Schmulik.AspNetClientSession.Cryptography
{
    /// <summary>
    /// Convenience class for creating a HMAC hash from any length data.
    /// </summary>
    public class Hmac
    {
        private readonly string _secretKey;
        private readonly StringBuilder _data;

        public Hmac(string secret)
        {
            _secretKey = secret;
            _data = new StringBuilder();
        }

        /// <summary>
        /// Add data to the buffer that will be hashed.
        /// </summary>
        /// <param name="value">The data to include</param>
        /// <returns>The Hmac instance for chaining</returns>
        public Hmac Update(string value)
        {
            _data.Append(value);
            return this;
        }

        /// <summary>
        /// Computes the HMAC hash, using the secret key provided.
        /// </summary>
        /// <returns>A Base64 encoded HMAC hash</returns>
        public string Digest()
        {
            var secret = Encoding.ASCII.GetBytes(_secretKey);
            var data = Encoding.ASCII.GetBytes(_data.ToString());
            using (var hmac = new HMACSHA256(secret))
            {
                var hash = hmac.ComputeHash(data);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Computes a HMAC hash for simple data, using the secret key provided.
        /// </summary>
        /// <param name="secret">The secret key</param>
        /// <param name="data">Simple data to hash</param>
        /// <returns>A Base64 encoded HMAC hash</returns>
        public static string Digest(string secret, string data)
        {
            return new Hmac(secret).Update(data).Digest();
        }
    }
}
