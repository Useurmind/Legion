using System.Collections.Generic;
using System.Linq;

namespace Legion.Core.Messages.InMemory
{
    public class InMemoryQueue
    {
        private List<object> messageHeaders = new List<object>();
        private List<object> messages = new List<object>();

        /// <summary>
        /// Add an message with associated headers.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="message"></param>
        /// <returns>The index of the new message.</returns>
        public int AddMessage(object header, object message)
        {
            if (header == null)
            {
                header = new EmptyMessageHeader();
            }

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
            if (header is EmptyMessageHeader)
            {
                return null;
            }

            return header;
        }

        public int LastIndex()
        {
            return this.messages.Count - 1;
        }
    }
}