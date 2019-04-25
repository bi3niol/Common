using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SMS.Exceptions
{
    public class ATCommandException : Exception
    {
        public string Command { get; private set; }
        public string ModemResponse { get; private set; }
        public string ErrorMessage { get; private set; }
        public override string Message => $"Command: {Command} {Environment.NewLine}Response: '{ModemResponse}' {Environment.NewLine} {ErrorMessage}";
        public ATCommandException(string command, string modemresponse, string errorMEssage) :base()
        {
            Command = command;
            ModemResponse = modemresponse;
            ErrorMessage = errorMEssage;
        }
    }
}
