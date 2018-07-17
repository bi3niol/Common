using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MongoDB.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CollectionNameAttribute : Attribute
    {
        public string CollectionName;
        public CollectionNameAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
