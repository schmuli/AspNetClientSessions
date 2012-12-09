using System;
using System.Dynamic;
using System.IO;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Schmulik.AspNetClientSession.Tests
{
    [TestClass]
    public class SessionTests
    {
        [TestMethod]
        public void Session_CanSetAndGetDynamicValues()
        {
            dynamic session = new Session();

            session.FirstName = "Schmulik";
            session.LastName = "Raskin";

            Assert.AreEqual("Schmulik", session.FirstName);
            Assert.AreEqual("Raskin", session.LastName);            
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Session_ReadingNonExistingValueThrowsException()
        {
            dynamic session = new Session();

            Assert.IsNull(session.FirstName);
        }

        [TestMethod]
        public void Session_CanClearAllValues()
        {
            dynamic my = new Session();

            my.FirstName = "First Name";

            Console.WriteLine(my.FirstName);

            var nondyn = my as Session;
            nondyn.Clear();
        }

        [TestMethod]
        public void Session_ParseSessionValues()
        {
            var session = new JsonSession
            {
                CreatedAt = DateTime.UtcNow,
                Duration = TimeSpan.FromDays(1),
                Values = new Dictionary<string, string>
                {
                    { "FirstName", "Schmulik" },
                    { "LastName", "Raskin" }
                }
            };

            var serializer = new JsonSerializer();
            var output = new StringWriter();
            serializer.Serialize(new JsonTextWriter(output), session);
            Console.WriteLine(output.ToString());

            var result = serializer.Deserialize<JsonSession>(new JsonTextReader(new StringReader(output.ToString())));
            
            Assert.AreEqual(session.CreatedAt, result.CreatedAt);
            Assert.AreEqual(session.Duration, result.Duration);
            Assert.AreEqual(session.Values.Count, result.Values.Count);
        }
        struct JsonSession
        {
            public DateTime CreatedAt { get; set; }
            public TimeSpan Duration { get; set; }
            public Dictionary<string, string> Values { get; set; }
        }
    }

}
