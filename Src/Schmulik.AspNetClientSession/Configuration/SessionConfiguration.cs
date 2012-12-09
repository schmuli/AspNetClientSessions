using System.Configuration;
using Schmulik.AspNetClientSession.Cryptography;
using System;

namespace Schmulik.AspNetClientSession.Configuration
{
    /// <summary>
    /// Contains configuration for Sessions, including default values.
    /// </summary>
    public class SessionConfiguration
    {
        /// <summary>
        /// The AppSettings key containing the Secret Key in the web.config.
        /// </summary>
        private const string SecretKeyConfig = "AspNetClientSession:SecretKey";
        /// <summary>
        /// The default cookie name, sent to the client.
        /// </summary>
        private const string DefaultCookieName = "AspNetClientSession";

        private string _secretKey;
        private string _cookieName;

        private TimeSpan? _duration;

        /// <summary>
        /// Gets or sets the Secret Key to use for encrypting/descrypting session data.
        /// </summary>
        public string SecretKey
        {
            get
            {
                return _secretKey;
            }
            set
            {
                _secretKey = value;
                UpdateKeys();
            }
        }

        /// <summary>
        /// The encryption key is generated based on the secret key, used to encrypt/decrypt
        /// session data.
        /// </summary>
        internal string EncryptionKey { get; set; }

        /// <summary>
        /// The signature key is generated based on the secret key, used to sign the HMAC hash.
        /// </summary>
        internal string SignatureKey { get; set; }

        /// <summary>
        /// Gets or sets the cookie name sent to the client. Defaults to "AspNetCookieName".
        /// </summary>
        public string CookieName
        {
            get
            {
                return _cookieName ?? DefaultCookieName;
            }
            set
            {
                _cookieName = value;
            }
        }

        /// <summary>
        /// The amount of time a session is kept alive. Defaults to 1 day.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return _duration ?? TimeSpan.FromDays(1);
            }
            set
            {
                _duration = value;
            }
        }

        public SessionConfiguration()
        {
            _secretKey = GetOrGenerateSecretKey();
        }

        /// <summary>
        /// Gets the secret key from the web.config, or generates a random per-application
        /// secret key.
        /// </summary>
        /// <returns>The secret key</returns>
        private string GetOrGenerateSecretKey()
        {
            _secretKey = ConfigurationManager.AppSettings[SecretKeyConfig];
            return !string.IsNullOrEmpty(_secretKey) ? _secretKey : GenerateSecretKey();
        }

        /// <summary>
        /// Generates a random secret key for the application.
        /// </summary>
        /// <para>
        /// Generated secret keys are only good for the life of the application on the machine
        /// that generated it. This means that anytime the application restarts or recycles,
        /// or in a shared-hosting environment, the secret key will be different.
        /// </para>
        /// <returns>A random generated secret key</returns>
        private string GenerateSecretKey()
        {
            _secretKey = RandomSecretKey.Generate();
            UpdateKeys();
            return _secretKey;
        }

        /// <summary>
        /// Updates the encryption and signature keys, anytime the secret key changes.
        /// </summary>
        private void UpdateKeys()
        {
            EncryptionKey = Hmac.Digest(_secretKey, "clientsession-encryption");
            SignatureKey = Hmac.Digest(_secretKey, "clientsession-signature");
        }
    }
}
