using System;
using System.Collections.Generic;
using System.Dynamic;
using DynamicObject = Schmulik.AspNetClientSession.Dynamic.DynamicObject;

namespace Schmulik.AspNetClientSession
{
    /// <summary>
    /// The Session data. Can be used with the 'dynamic' keyword, to get and set
    /// properties, or with the 'var' keyword, to clear all properties.
    /// </summary>
    public class Session : DynamicObject
    {
        /// <summary>
        /// The underlying dictionary containing the dynamic properties.
        /// </summary>
        internal Dictionary<string, string> Values { get; set; }

        /// <summary>
        /// Tracks updates. If the data has not been changed, it won't get
        /// sent to the client again.
        /// </summary>
        internal bool IsDirty { get; private set; }

        /// <summary>
        /// The time the session was created, used to reset an expired session.
        /// </summary>
        internal DateTime CreatedAt { get; set; }

        /// <summary>
        /// The amount of time a session is kept alive.
        /// </summary>
        internal TimeSpan Duration { get; set; }

        public Session()
        {
            CreatedAt = DateTime.UtcNow;
            Values = new Dictionary<string, string>();
        }

        internal Session(TimeSpan duration)
            : this()
        {
            Duration = duration;
        }

        /// <summary>
        /// Clears any dynamically added properties.
        /// </summary>
        public void Clear()
        {
            if (Values.Count > 0)
            {
                Values.Clear();
                IsDirty = true;
            }
        }

        /// <summary>
        /// Resets the session creation, and marks it as updated.
        /// </summary>
        /// <param name="duration"></param>
        internal void Reset(TimeSpan duration)
        {
            Values.Clear();
            CreatedAt = DateTime.UtcNow;
            Duration = duration;
            IsDirty = true;
        }

        /// <summary>
        /// Dynamically retrieves a property. If the property doesn't exist, returns null.
        /// </summary>
        /// <para>
        /// For now, only string properties are supported.
        /// </para>
        /// <param name="binder"></param>
        /// <param name="result">The property value, or null if it doesn't exist</param>
        /// <returns>always returns true</returns>
        protected override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string value;
            Values.TryGetValue(binder.Name, out value);
            result = value;
            return true;
        }

        /// <summary>
        /// Dynamically creates or updates a property. Only marks as dirty if creating a 
        /// new property or setting a different value for an existing property.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value">The value to set</param>
        /// <returns>always returns true</returns>
        protected override bool TrySetMember(SetMemberBinder binder, object value)
        {
            string existing;
            if (!Values.TryGetValue(binder.Name, out existing) && existing != value.ToString())
            {
                Values[binder.Name] = value.ToString();
                IsDirty = true;
            }
            return true;
        }
    }
}
