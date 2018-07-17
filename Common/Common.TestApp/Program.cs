using Common.Model;
using Common.MongoDB.Attributes;
using Common.MongoDB.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TestApp
{
    [CollectionName("TestCollection")]
    class TestClass:Entity<ObjectId>
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"{Id} - {Age} - {Name}";
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(TypeHelper.GetCollectionNameFromType<TestClass>());
            var client = new MongoClient();
            var db = client.GetDatabase("testDataBase");
            var repo = new MongoDB.Repository<TestClass>(db, true);
            Console.WriteLine("Wszystkie Elementy");
            foreach (var item in repo.GetAll())
            {
                Console.WriteLine(item);
            }
            if(Console.ReadLine() == "i")
            {
                Console.WriteLine("Dodaj nowy");

                repo.Add(new TestClass
                {
                    Age = Int32.Parse(Console.ReadLine()),
                    Name = Console.ReadLine()
                });
            }
            if (Console.ReadLine() == "u")
            {
                Console.WriteLine("Aktualizuj...");
                Console.WriteLine("podaj Id");
                ObjectId id = new ObjectId(Console.ReadLine());
                var uel = repo.GetEntity(id);
                Console.WriteLine("podaj nowe imie");
                uel.Name = Console.ReadLine();
                repo.Update(uel);
            }
        }
    }
}
