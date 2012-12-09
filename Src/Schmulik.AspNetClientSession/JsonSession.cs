using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Schmulik.AspNetClientSession
{
    /// <summary>
    /// Used to serialize/deserialize session data.
    /// </summary>
    internal struct SessionSerializer
    {
        public DateTime CreatedAt { get; set; }
        public TimeSpan Duration { get; set; }
        public Dictionary<string, string> Values { get; set; }

        /// <summary>
        /// Deserializes session data from JSON.
        /// </summary>
        /// <param name="values">The JSON representation</param>
        /// <returns>A new Session with data</returns>
        internal static Session Deserialize(string values)
        {
            using (var reader = new JsonTextReader(new StringReader(values)))
            {
                var serializer = new JsonSerializer();
                var result = serializer.Deserialize<SessionSerializer>(reader);

                return new Session
                {
                    CreatedAt = result.CreatedAt,
                    Duration = result.Duration,
                    Values = result.Values,
                };
            }
        }

        /// <summary>
        /// Serializes session data into JSON.
        /// </summary>
        /// <param name="session">The Session data</param>
        /// <returns>A JSON representation</returns>
        internal static string Serialize(Session session)
        {
            var values = new SessionSerializer
            {
                CreatedAt = session.CreatedAt,
                Duration = session.Duration,
                Values = session.Values
            };

            using (var writer = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(jsonWriter, values);
                return writer.ToString();
            }
        }
    }
}