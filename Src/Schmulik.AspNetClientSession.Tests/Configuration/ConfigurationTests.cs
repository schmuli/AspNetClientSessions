using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schmulik.AspNetClientSession.Configuration;

namespace Schmulik.AspNetClientSession.Tests.Configuration
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void Configuration_UseDefaultSecretKey()
        {
            var configuration = new SessionConfiguration();

            Assert.IsNotNull(configuration.SecretKey);
            Console.WriteLine(configuration.SecretKey);
        }

        [TestMethod]
        public void Configuration_ReadSecretKeyFromConfig()
        {
            var configuration = new SessionConfiguration();

            Assert.IsNotNull(configuration.SecretKey);
            Assert.AreEqual("a secret key", configuration.SecretKey);
            Console.WriteLine(configuration.SecretKey);
        }

        [TestMethod]
        public void Configuration_SetSecretKeyFromCode()
        {
            var configuration = new SessionConfiguration { SecretKey = "code set secret key" };

            Assert.IsNotNull(configuration.SecretKey);
            Assert.AreEqual("code set secret key", configuration.SecretKey);
        }

        [TestMethod]
        public void Configuration_UseDefaultCookieName()
        {
            var configuration = new SessionConfiguration();

            Assert.IsNotNull(configuration.CookieName);
            Console.WriteLine(configuration.CookieName);
        }

        [TestMethod]
        public void Configuration_SetCookieName()
        {
            var configuration = new SessionConfiguration { CookieName = "MyCookieName" };

            Assert.IsNotNull(configuration.CookieName);
            Assert.AreEqual("MyCookieName", configuration.CookieName);
        }

        [TestMethod]
        public void Configuration_UseDefaultDuration()
        {
            var configuration = new SessionConfiguration();
            
            Assert.AreEqual(TimeSpan.FromDays(1), configuration.Duration);
        }

        [TestMethod]
        public void Configuration_SetDuration()
        {
            var configuration = new SessionConfiguration()
            {
                Duration = TimeSpan.FromDays(3)
            };

            Assert.AreEqual(TimeSpan.FromDays(3), configuration.Duration);
            Assert.AreNotEqual(TimeSpan.FromDays(1), configuration.Duration);
        }
    }
}
