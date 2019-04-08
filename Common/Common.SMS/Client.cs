using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using static Common.SMS.ATCommands;
namespace Common.SMS
{
    public class Client : IDisposable
    {
        private SerialPort serialPort;
        public Client(SerialPort port)
        {
            serialPort = port;
        }

        public Client(string comPortName)
        {
            if (!SerialPort.GetPortNames().Contains(comPortName))
                throw new ArgumentException($"No COM port with name {comPortName}");
            serialPort = new SerialPort(comPortName);
            SetParameters();
        }

        public bool SendMessage(PhoneNumber phone, Message message)
        {
            SetParameters(bitesPerCharacter: message.DataBits);
            if (!serialPort.IsOpen)
                serialPort.Open();
            ExecuteCommand(AT);
            ExecuteCommand(AT_CMGF(1));
            ExecuteCommand(AT_CSCS("\"GSM\""));
            ExecuteCommand(AT_CMGS(phone));
            ExecuteCommand(message.ToString(), newLine: false);
            ExecuteCommand(CTRL_Z);
            return true;
        }

        private void SetParameters(Parity parity = Parity.None, BitesPerCharacter bitesPerCharacter = BitesPerCharacter.Eight,
            StopBits stopBits = StopBits.One, Handshake handshake = Handshake.RequestToSend, bool dtrEnable = true, bool rtsEnable = true)
        {
            serialPort.Parity = parity;
            serialPort.DataBits = (int)bitesPerCharacter;
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
            if (res != null && res.Contains("Error"))
            {
                throw new Exception(errorMEssage??res);
            }
            return res;
        }

        public void Dispose()
        {
            serialPort.Dispose();
        }
    }
}
