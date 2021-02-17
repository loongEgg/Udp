using System.Text;

namespace LoongEgg.Udp
{
    public delegate void ReceiveEvent(object sender, ReceiveEventArgs args);

    public class ReceiveEventArgs
    {
        public byte[] Buffer { get; }

        public string Message { get; }

        public ReceiveEventArgs(byte[] buffer)
        {
            Buffer = buffer;
            Message = Encoding.UTF8.GetString(Buffer);
        }
    }
}