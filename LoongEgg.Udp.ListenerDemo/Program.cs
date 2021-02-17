using System;

namespace LoongEgg.Udp.ListenerDemo
{
    class Program
    {
        static void Main(string[] args)
        { 
            var listener = new UdpListener();
            listener.Open();
        }
    }
}
