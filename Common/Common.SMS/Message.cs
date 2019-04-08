using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Common.SMS
{
    public enum BitesPerCharacter
    {
        Eight = 8,
        Sixteen = 16
    }
    public class Message : IEnumerable<byte>, IEnumerable
    {
        public BitesPerCharacter DataBits { get; }

        public Message(string content, BitesPerCharacter bitesPerCharacter):this(content.GetSmsContent(bitesPerCharacter),bitesPerCharacter)
        {
        }
        public Message(byte[] content, BitesPerCharacter bitesPerCharacter)
        {
            DataBits = bitesPerCharacter;
            Content = content;
            NumberOfSubmessages = Length / Values.BytesPerSubmessage + (Length % Values.BytesPerSubmessage > 0 ? 1 : 0);
        }

        public int NumberOfSubmessages { get; }

        public int Length
        {
            get => Content.Length;
        }

        public byte[] Content
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Content.GetStringSmsContent(DataBits);
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return Content.AsEnumerable<byte>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}