using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SMS
{
    public static class Values
    {
        public static int MaxSingleMessageLengthInBites { get; internal set; } = 1120;
        public static int BytesPerSubmessage { get; internal set; } = 150;
    }
}
