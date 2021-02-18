using System.Net;
using System.Text;

namespace LoongEgg.Udp
{
    public delegate void ReceiveEvent(object sender, ReceiveEventArgs args);

    public class ReceiveEventArgs
    {
        public byte[] Buffer { get; }

        public string Message { get; }

        public string Ip { get; } = string.Empty;

        public string Port { get; } = string.Empty;

        public ReceiveEventArgs(byte[] buffer, IPEndPoint endPoint)
        {
            Buffer = buffer;
            Message = Encoding.UTF8.GetString(Buffer);
            Ip = endPoint.Address.ToString();
            Port = endPoint.Port.ToString();
        }
    }
}