using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Legion.Core.Messages.Types
{
    /// <summary>
    /// A registry that stores the different message types.
    /// They are keyed by a string, messageTypeName, that can be used to retrieve the matching types
    /// for messages with that key.
    /// </summary>
    public class MessageTypeRegistry : IMessageTypeRegistry
    {
        private Dictionary<string, Type> messageTypesByName = new Dictionary<string, Type>();

        public MessageTypeRegistry(IEnumerable<Assembly> assembliesToScan)
        {
            foreach (var assembly in assembliesToScan)
            {
                var messageTypes = assembly.GetTypes().Where(t => t.GetCustomAttribute(typeof(MessageAttribute)) != null);
                
                this.messageTypesByName = messageTypes.ToDictionary(this.GetMessageTypeName, x => x);
            }
        }

        public Type GetMessageType(string messageTypeName)
        {
            Type messageType = null;
            if (!this.messageTypesByName.TryGetValue(messageTypeName, out messageType))
            {
                throw new Exception($"Could not find type of message for message with type name {messageTypeName}");
            }

            return messageType;
        }

        public string GetMessageTypeName(Type messageType)
        {
            var messageTypeNameAttribute = messageType.GetCustomAttribute<MessageTypeNameAttribute>();
            if (messageTypeNameAttribute != null && !string.IsNullOrEmpty(messageTypeNameAttribute.TypeName))
            {
                return messageTypeNameAttribute.TypeName;
            }

            return messageType.Name;
        }
    }
}