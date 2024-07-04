using System;
using System.Runtime.Serialization;

namespace mmDailyPlanner.Server.Exceptions
{
    [Serializable]
    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException() : base() { }
        public DatabaseOperationException(string message) : base(message) { }
        public DatabaseOperationException(string message, Exception innerException) : base(message, innerException) { }
        protected DatabaseOperationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
