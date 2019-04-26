using Common.Model;
using Common.MongoDB;
using Common.MongoDB.Attributes;
using Common.MongoDB.Helpers;
using Common.SMS;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TestApp
{
    [CollectionName("TestCollection")]
    [BsonIgnoreExtraElements]
    class TestClass:Entity<ObjectId>
    {
        public int? Age { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"{Id} - {Age} - {Name}";
        }
    }

    [CollectionName("TestCollection")]
    [BsonIgnoreExtraElements]
    class ChildTestClass : Entity<ObjectId>
    {
        public int? TypeInt { get; set; }
    }
    class Program
    {
        static void SmsTest()
        {
            foreach (var port in SerialPort.GetPortNames())
            {
                Trace.WriteLine(port);
            }
            Message msg = new Message(@"No witam serdecznie jak tam zdrowie
wlasnie appke do wysylania sms :D, 
wlasnie sprawdzamy czy dziala wyslanie dlugich wiadomosci.
Mam nadzieje e bedzie git :D, juz prawie jest wystarczajaco ilosc znakow na to
panowie testujemy 
", BitesPerCharacter.Eight);
            Message msg2 = new Message(@"No witam serdecznie jak tam zdrowie", BitesPerCharacter.Eight);
            Trace.WriteLine($"ilosc subwiadomosci {msg.NumberOfSubmessages}");
            for (int i = 1; i <= msg.NumberOfSubmessages; i++)
            {
                var submsg = msg.GetPartOfMessage(i);
                Trace.WriteLine(submsg.Length);
                Trace.WriteLine(submsg);
            }
            SerialPort sp = new SerialPort();
            Trace.WriteLine(sp.Encoding);
            //return;
            SMS.ATCommandsClient sms = new ATCommandsClient("COM6");
            sms.SendMessage(new PhoneNumber("535315310"), msg);
        }
        static void Main(string[] args)
        {
            MongoDB();
            return;
            SMS.ATCommandsClient sms = new ATCommandsClient("COM6");
            sms.SendMessage(new PhoneNumber(args[1]), new Message(args[2], BitesPerCharacter.Eight));
        }

        static void MongoDB()
        {
            Console.WriteLine(TypeHelper.GetCollectionNameFromType<ChildTestClass>());
            Console.WriteLine(TypeHelper.GetCollectionNameFromType<TestClass>());
            MongoClient client = new MongoClient();
            var db = client.GetDatabase("MongoTest");
            Repository<ChildTestClass> repository1 = new Repository<ChildTestClass>(db, true);
            Repository<TestClass> repository2 = new Repository<TestClass>(db, true);
            repository2.Add(new TestClass
            {
                Name = "franek"
            });
            foreach (var item in repository1.GetAll())
            {
                Console.WriteLine($"{item.Id} - {item.TypeInt}");
            }
            foreach (var item in repository2.GetAll())
            {
                Console.WriteLine($"{item.Id} - {item.Name}");
            }
        }
    }
}
