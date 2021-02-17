using LoongEgg.Log;
using LoongEgg.SharpExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LoongEgg.Udp
{
    /// <summary>
    /// Udp发送端
    /// </summary>
    public class UdpSender : BaseUdpClient
    {
        /// <summary>
        /// 在控制台显示发送记录.[default]=true, set at ctor.
        /// </summary>
        /// <remarks>
        ///     注意:置为true时会打开<see cref="Loggers.Console"/>并且使能log打印; 
        ///     但是置为false时不会关闭<see cref="Loggers.Console"/>而只会禁止打印输出.
        /// </remarks>
        public static bool LogEnabled
        {
            get { return _LogEnabled; }
            set
            {
                _LogEnabled = value;
                if (LogEnabled)
                {
                    Logger.Enable(Loggers.Console);
                }
            }
        }
        private static bool _LogEnabled;
        private Socket _Socket;
         
        static UdpSender() { LogEnabled = true; }

        /// <summary>
        /// 生成一个<see cref="UdpSender"/>实例
        /// </summary>
        /// <param name="ip">远程Ip地址.[default]="127.0.0.1", 为回环模式即本机同一地址接收and发送</param>
        /// <param name="port">远程端口号.[default]=11000</param>
        public UdpSender(string ip = "127.0.0.1", uint port = 11000) : base(ip, port) { }

        public override void Close()
        {
            IsOpen = false;
            _Socket?.Close();
            _Socket = null;
        }

        public override void Open()
        {
            if (!CanOpen) return;

            try
            {
                _IpEndPointRemote = new IPEndPoint(_IpAddressRemote, (int)PortRemote);
                _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                if (LogEnabled)
                {
                    Logger.Info($"Local ip [{IpLocal}]");
                    Logger.Info($"Udp sender open to [{_IpEndPointRemote.Address}: {_IpEndPointRemote.Port}]");
                }
                IsOpen = true;
            }
            catch (Exception ex)
            {
                if (LogEnabled)
                {
                    Logger.Erro($"Udp open error: {ex.Message}");
                }
                Close();
                throw ex;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="buff"></param>
        public void Send(IEnumerable<byte> buff)
        {
            if (!IsOpen) Open();

            if (buff.Any())
            {
                int length = buff.Count();
                Buffer = new byte[length];
                Array.Copy(buff.ToArray(), Buffer, length);
                length = _Socket.SendTo(Buffer, _IpEndPointRemote);
                if (LogEnabled)
                {
                    Logger.Info($"Udp sent hex[{length}]: {Buffer.ToHexString(' ')}");
                }
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">UTF8编码的字符串</param>
        public void Send(string msg)
        {
            if (LogEnabled && msg.Any())
            {
                Logger.Info($"Udp sent msg: {msg}");
            }
            Send(Encoding.UTF8.GetBytes(msg));
        }

        #region GC
        private bool _Diposed = false;
        ~UdpSender()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_Diposed)
            {
                if (disposing)
                {
                    Close();
                }
                _Diposed = true;
            }
        }
        #endregion
    }
}
