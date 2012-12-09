using System;

namespace Schmulik.AspNetClientSession.Cryptography
{
    /// <summary>
    /// Converts Base64 strings to and from URL safe strings.
    /// </summary>
    public class Base64Url
    {
        /// <summary>
        /// Encodes a URL safe Base64 string.
        /// </summary>
        /// <param name="value">A Base64 string</param>
        /// <returns>A URL safe Base64 string</returns>
        public static string Encode(string value)
        {
            return value.Replace('/', '_').Replace('+', '-').Replace("=", "");
        }

        /// <summary>
        /// Decodes a URL safe Base64 string.
        /// </summary>
        /// <param name="value">A URL safe Base64 string</param>
        /// <returns>A valid Base64 string</returns>
        public static string Decode(string value)
        {
            value = value.Replace('_', '/').Replace('-', '+');
            switch (value.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    value += "==";
                    break;
                case 3:
                    value += "=";
                    break;
                default:
                    throw new ArgumentException("Illegal Base64Url string");
            }
            return value;
        }
    }
}
