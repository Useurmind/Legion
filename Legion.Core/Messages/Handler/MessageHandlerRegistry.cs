using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Legion.Core.Messages.Handler
{
    /// <summary>
    /// 
    /// 1. Scan for message handler types and tell container what types were found.
    /// 2. Connect to container resolution mechanism.
    /// </summary>
    public class MessageHandlerRegistry : IMessageHandlerRegistry
    {
        private Dictionary<Type, IEnumerable<Type>> messageHandlerTypesByMessageType = new Dictionary<Type, IEnumerable<Type>>();
        private Func<Type, IMessageHandler> resolveMessageHandler;

        /// <summary>
        /// Create an instance.
        /// </summary>
        public MessageHandlerRegistry()
        {
            
        }

        /// <summary>
        /// Scan assemblies for message handler types.
        /// </summary>
        /// <param name="assembliesToScan">The assemblies to scan for message handlers.</param>
        /// <param name="registerMessageHandler">This is a function that can be provided by a container to register message handler types with it.</param>
        public void ScanForMessageHandlerTypes(IEnumerable<Assembly> assembliesToScan, Action<Type> registerMessageHandler)
        {
            var messageHandlerTypes = Assembly.GetAssembly(this.GetType()).GetTypes().Where(t => t.GetCustomAttribute(typeof(MessageHandlerAttribute)) != null);
            this.messageHandlerTypesByMessageType = messageHandlerTypes.GroupBy(x => GetMessageTypeOfHandlerType(x))
                                                                       .ToDictionary(x => x.Key, x => (IEnumerable<Type>)x);

            foreach (var messageHandlerType in messageHandlerTypes)
            {
                registerMessageHandler(messageHandlerType);
            }
        }

        /// <summary>
        /// This function servers as integration point with the dependency injection container.
        /// The container will call this when creating the registry.
        /// The given function can be used to create a message handler for a specific event type directly from the container.
        /// This is nice to get dependencies into the message handlers.
        /// </summary>
        /// <param name="resolveMessageHandler">Resolve a message handler.</param>
        public void ConnectContainerResolution(Func<Type, IMessageHandler> resolveMessageHandler) {
            this.resolveMessageHandler = resolveMessageHandler;
        }

        /// <inheritdoc />
        public IEnumerable<IMessageHandler> GetMessageHandler(Type messageType)
        {
            IEnumerable<Type> messageHandlerTypes;
            if (!this.messageHandlerTypesByMessageType.TryGetValue(messageType, out messageHandlerTypes))
            {
                throw new Exception($"Could not find types for message handlers for message with type name {messageType.Name}");
            }

            return messageHandlerTypes.Select(x => this.resolveMessageHandler(x));
        }

        private static Type GetMessageTypeOfHandlerType(Type handlerType) {
            return handlerType.GetInterfaces().First(x => x.Name.StartsWith("IMessageHandler") && x.GenericTypeArguments.Length == 1)
                              .GenericTypeArguments[0];
        }
    }
}