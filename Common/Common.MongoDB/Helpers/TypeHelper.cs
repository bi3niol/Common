using Common.MongoDB.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MongoDB.Helpers
{
    public class TypeHelper
    {
        public static string GetCollectionNameFromType<T>()
        {
            Type type = typeof(T);
            string collectionName = null;
            CollectionNameAttribute collectionNameAttribute =Attribute.GetCustomAttribute(type, typeof(CollectionNameAttribute)) as CollectionNameAttribute;
            if (collectionNameAttribute != null)
                collectionName = collectionNameAttribute.CollectionName;
            else
                collectionName = type.Name;
            return collectionName;
        }
    }
}
