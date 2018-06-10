using System.Collections.Generic;
using System.Linq;

namespace Legion.Core.Messages.InMemory
{
    public class InMemoryQueue
    {
        private List<object> messageKeys = new List<object>();
        private List<object> messageHeaders = new List<object>();
        private List<object> messages = new List<object>();

        /// <summary>
        /// Add an message with associated headers.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="header"></param>
        /// <param name="message"></param>
        /// <returns>The index of the new message.</returns>
        public int AddMessage(object key, object header, object message)
        {
            if (header == null)
            {
                header = new EmptyMessageObject();
            }
            if (key == null)
            {
                key = new EmptyMessageObject();
            }

            this.messageKeys.Add(key);
            this.messageHeaders.Add(header);
            this.messages.Add(message);

            return this.LastIndex();
        }

        public object GetMessage(int index)
        {
            return this.messages[index];
        }

        public object GetMessageHeader(int index)
        {
            var header = this.messageHeaders[index];
            if (header is EmptyMessageObject)
            {
                return null;
            }

            return header;
        }

        public object GetMessageKey(int index)
        {
            var key = this.messageKeys[index];
            if (key is EmptyMessageObject)
            {
                return null;
            }

            return key;
        }

        public int LastIndex()
        {
            return this.messages.Count - 1;
        }
    }
}