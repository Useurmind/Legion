using System;
using System.Linq;

namespace Legion.Core.Messages.Types
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class MessageTypeNameAttribute : Attribute
    {
        /// <summary>
        /// The name of the message type under which the message can be identified
        /// in the message store.
        /// </summary>
        public string TypeName { get; }

        public MessageTypeNameAttribute(string typeName)
        {
            this.TypeName = typeName;
        }
    }
}