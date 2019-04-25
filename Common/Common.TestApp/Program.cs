using Common.Model;
using Common.MongoDB.Attributes;
using Common.MongoDB.Helpers;
using Common.SMS;
using MongoDB.Bson;
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
            return;
            sms.SendMessage(new PhoneNumber(args[1]), new Message(args[2], BitesPerCharacter.Eight));
        }
    }
}
