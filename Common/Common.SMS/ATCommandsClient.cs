using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using static Common.SMS.ATCommands;
using System.Diagnostics;
using Common.SMS.Exceptions;

namespace Common.SMS
{
    public class ATCommandsClient : IDisposable
    {
        private SerialPort serialPort;
        public ATCommandsClient(SerialPort port)
        {
            serialPort = port;
        }

        public ATCommandsClient(string comPortName)
        {
            if (!SerialPort.GetPortNames().Contains(comPortName))
                throw new ArgumentException($"No COM port with name {comPortName}");
            serialPort = new SerialPort(comPortName);
            SetParameters();
        }

        public bool SendMessage(PhoneNumber phone, Message message)
        {
            if (message.DataBits == BitesPerCharacter.Sixteen)
                throw new NotSupportedException("Unicode message character not supported");
            SetParameters(bitesPerCharacter: message.DataBits);
            if (!serialPort.IsOpen)
                serialPort.Open();
            int refNumber = Math.Abs(message.GetHashCode()) % 256;
            int iei = (int)message.DataBits - 8;
            ExecuteCommand(AT);
            ExecuteCommand(AT_CMGF(1));
            ExecuteCommand(AT_CSCS("\"GSM\""));
            if (message.NumberOfSubmessages > 1)
            {
                ExecuteCommand(DCD(1));
                for (int seq = 1; seq <= message.NumberOfSubmessages; seq++)
                {
                    ExecuteCommand(AT_UCMGS(phone, seq, message.NumberOfSubmessages, iei, refNumber));
                    ExecuteCommand(AT_MSG(message.GetPartOfMessage(seq)));
                }
                ExecuteCommand(DCD(0));
            }
            else
            {
                ExecuteCommand(DCD(0));
                ExecuteCommand(AT_CMGS(phone));
                ExecuteCommand(AT_MSG(message.ToString()));
            }
            return true;
        }

        private void SetParameters(Parity parity = Parity.None, BitesPerCharacter bitesPerCharacter = BitesPerCharacter.Eight,
            StopBits stopBits = StopBits.One, Handshake handshake = Handshake.RequestToSend, bool dtrEnable = true, bool rtsEnable = true)
        {
            serialPort.Parity = parity;
            serialPort.DataBits = 8;
            serialPort.StopBits = stopBits;
            serialPort.Handshake = handshake;
            serialPort.DtrEnable = dtrEnable;
            serialPort.RtsEnable = rtsEnable;
            serialPort.Encoding = Helpers.GetEncoding(bitesPerCharacter);
            serialPort.NewLine = Environment.NewLine;
        }

        private string ExecuteCommand(string command, string errorMEssage = null, bool newLine = true, int sleep = 100)
        {
            serialPort.Write(command + (newLine ? serialPort.NewLine : ""));
            System.Threading.Thread.Sleep(sleep);
            var res = serialPort.ReadExisting();
            Trace.TraceInformation("AT Command : "+command);
            Trace.TraceInformation("AT Response : "+res);
            if (res != null && res.ToLower().Contains("error"))
            {
                throw new ATCommandException(command, res, errorMEssage);
            }
            return res;
        }

        public void Dispose()
        {
            serialPort.Dispose();
        }
    }
}
