using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SMS
{
    internal static class Helpers
    {
        public static Encoding GetEncoding(BitesPerCharacter bitesPerCharacter)
        {
            switch (bitesPerCharacter)
            {
                case BitesPerCharacter.Eight:
                    return Encoding.ASCII;
                case BitesPerCharacter.Sixteen:
                    return Encoding.Unicode;
                default:
                    break;
            }
            throw new NotSupportedException("bitesPerCharacter");
        }
    }
}
