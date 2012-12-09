using Schmulik.AspNetClientSession.Configuration;
using Schmulik.AspNetClientSession.Cryptography;
using System;
using System.Web;

namespace Schmulik.AspNetClientSession
{
    /// <summary>
    /// Handles decryption/encryption of Session Data, and makes the Session data
    /// available on the HTTP Context.
    /// </summary>
    public static class ClientSession
    {
        /// <summary>
        /// A private key for the session data.
        /// </summary>
        private static readonly object ItemKey = new object();

        /// <summary>
        /// Gets the session configuration. Access in Applicaton_Start to set prefered
        /// values.
        /// </summary>
        public static SessionConfiguration Configuration { get; private set; }

        static ClientSession()
        {
            Configuration = new SessionConfiguration();
        }

        /// <summary>
        /// Handles decryption of the Session cookie value. Resets the session after
        /// expiration.
        /// </summary>
        /// <param name="context"></param>
        public static void ProcessPreRequest(HttpContextBase context)
        {
            var cookies = new CookieHelper(Configuration.CookieName);
            var encrypted = cookies.Gets(context);
            if (string.IsNullOrEmpty(encrypted))
            {
                return;
            }

            var encryption = new Encryption(Configuration.EncryptionKey, Configuration.SignatureKey);
            var decrypted = encryption.Decrypt(encrypted);
            if (string.IsNullOrEmpty(decrypted))
            {
                return;
            }

            var session = SessionSerializer.Deserialize(decrypted);
            if (session.CreatedAt + session.Duration < DateTime.UtcNow)
            {
                session.Reset(Configuration.Duration);
            }

            context.Items[ItemKey] = session;
        }

        /// <summary>
        /// Handles encryption of the session data into a HTTP cookie.
        /// </summary>
        /// <param name="context"></param>
        public static void ProcessPostRequest(HttpContextBase context)
        {
            var session = GetSession(context, false);
            if (session == null || !session.IsDirty)
            {
                return;
            }

            var plaintext = SessionSerializer.Serialize(session);
            var encryption = new Encryption(Configuration.EncryptionKey, Configuration.SignatureKey);
            var encrypted = encryption.Encrypt(plaintext);

            var cookies = new CookieHelper(Configuration.CookieName);
            cookies.Set(context, encrypted);
        }

        /// <summary>
        /// (Extension) Retrieves the current Session data from the HTTP context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>The current Session data</returns>
        public static Session Session(this HttpContextBase context)
        {
            return GetSession(context, true);
        }

        internal static void ProcessPreRequest(HttpApplication application)
        {
            ProcessPreRequest(new HttpContextWrapper(application.Context));
        }

        internal static void ProcessPostRequest(HttpApplication application)
        {
            ProcessPostRequest(new HttpContextWrapper(application.Context));
        }

        /// <summary>
        /// Gets the session data from the current context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="create">If true, creates a new Session if it doesn't exist</param>
        /// <returns>The Session data</returns>
        private static Session GetSession(HttpContextBase context, bool create)
        {
            var session = context.Items[ItemKey] as Session;
            if (session == null && create)
            {
                session = new Session(Configuration.Duration);
                context.Items[ItemKey] = session;
            }
            return session;
        }
    }
}
