using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SMS
{
    public static class StringExtensions
    {
        public static byte[] GetSmsContent(this string _this, BitesPerCharacter bitesPerCharacter) => Helpers.GetEncoding(bitesPerCharacter).GetBytes(_this);
        public static string GetStringSmsContent(this IEnumerable<byte> _this, BitesPerCharacter bitesPerCharacter) => _this.ToArray().GetStringSmsContent(bitesPerCharacter);
        public static string GetStringSmsContent(this byte[] _this, BitesPerCharacter bitesPerCharacter) => Helpers.GetEncoding(bitesPerCharacter).GetString(_this);
    }
}
