using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Linq;
using IdentityServer4.Models;

namespace Accounting.Domain
{
    [Serializable]
    public class InvalidRangeException : Exception
    {
        public InvalidRangeException() { }
        public InvalidRangeException(string message) : base(message) { }
        protected InvalidRangeException(SerializationInfo serializationInfo, StreamingContext streamingContext) => throw new NotImplementedException();
    }

    [Serializable]
    public class CustomApplicationException : Exception
    {
        public CustomApplicationException() { }
        public CustomApplicationException(string message) : base(message) { }
        protected CustomApplicationException(SerializationInfo serializationInfo, StreamingContext streamingContext) => throw new NotImplementedException();
    }
}
