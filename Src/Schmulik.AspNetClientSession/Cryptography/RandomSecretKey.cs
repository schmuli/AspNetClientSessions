using System;
using System.Security.Cryptography;

namespace Schmulik.AspNetClientSession.Cryptography
{
    /// <summary>
    /// Convenience method, for creating a random secret key.
    /// </summary>
    public class RandomSecretKey
    {
        /// <summary>
        /// Creates a random secret key, using a 128-bit salt.
        /// </summary>
        /// <returns>A Base64 encoded secret key</returns>
        public static string Generate()
        {
            using (var rng = new Rfc2898DeriveBytes("random password", 32))
            {
                var secretKeyBytes = rng.GetBytes(32);
                var result = Convert.ToBase64String(secretKeyBytes);
                return Base64Url.Encode(result);
            }
        }
    }
}
