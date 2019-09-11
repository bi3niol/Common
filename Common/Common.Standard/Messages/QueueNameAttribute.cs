using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Messages
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false)]
    public class QueueNameAttribute:Attribute
    {
        public QueueNameAttribute(string queueName)
        {
            QueueName = queueName;
        }

        public string QueueName { get; set; }
    }
}
