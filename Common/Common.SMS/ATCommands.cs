using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SMS
{
    public static class ATCommands
    {
        public static string AT { get; } = "AT";
        public static string AT_CMGF { get; } = "AT+CMGF={0}";
        public static string AT_CSCS { get; } = "AT+CSCS={0}";
        public static string AT_CMGS { get; } = "AT+CMGS=\"{0}\"";
        public static string CTRL_Z { get; } = ""+(char)(26);
    }
}
