using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Standard.Types
{
    public class CommonException : Exception
    {
        public CommonException()
        {
        }

        public CommonException(string message) : base(message)
        {
        }

        public CommonException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CommonException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
