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
        public static string AT_CSCS(object param) => string.Format("AT+CSCS={0}",param);
        public static string AT_CMGS(object param) => string.Format("AT+CMGS=\"{0}\"",param);
        public static string CTRL_Z { get; } = ""+(char)(26);
        public static string AT_CMGF(object param) => string.Format("AT+CMGF={0}",param);
        public static string AT_UCMGS(object recipent,int seq, int max,int iei, int _ref, int? toda = null) => $"AT+UCMGF=\"{recipent}\",{toda},{seq},{max},{iei},{_ref}";// string.Format("AT+UCMGF={0}", param);
        public static string AT_MSG(string param) => $"{param}{CTRL_Z}"; 
        public static string DCD(int param) => $"DCD={param}";
    }
}
