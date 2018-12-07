using System;
using System.Runtime.Serialization;

namespace DIContainers
{
    public class DIContainerException : Exception
    {
        public DIContainerException()
        {
        }

        public DIContainerException(string message) : base(message)
        {
        }

        public DIContainerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DIContainerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
