using System;

namespace LoongEgg.Udp.SenderDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var sender = new UdpSender();
            string msg;

            sender.Open();
            Console.WriteLine("Input any string to send but [stop] to exist");
            do
            {
                msg = Console.ReadLine();
                sender.Send(msg);
                if (msg.ToLower() == "stop")
                    break;
            } while (true);
            sender.Close();
        }
    }
}
