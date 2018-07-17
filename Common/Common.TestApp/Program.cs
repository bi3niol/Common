using Common.MongoDB.Attributes;
using Common.MongoDB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TestApp
{
    [CollectionName("TestCollection")]
    class TestClass
    {

    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(TypeHelper.GetCollectionNameFromType<TestClass>());
        }
    }
}
