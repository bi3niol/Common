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

        /// <summary>
        /// start from ID = 1
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public string GetPartOfMessage(int partId)
        {
            return Content.Skip((partId - 1) * Values.BytesPerSubmessage).Take(Values.BytesPerSubmessage).GetStringSmsContent(DataBits);
        }
        public int Length
        {
            get => Content.Length;
        }

        public byte[] Content
        {
            get;
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

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}