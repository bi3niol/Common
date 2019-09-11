using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Messages
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RoutingKeyAttribute : Attribute
    {
        public string RoutingKey { get; set; }

        public RoutingKeyAttribute(string routingKey)
        {
            RoutingKey = routingKey;
        }
    }
}
