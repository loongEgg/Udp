using LoongEgg.Log;
using LoongEgg.SharpExtensions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LoongEgg.Udp
{
    /// <summary>
    /// Udp接收端
    /// </summary>
    public sealed class UdpListener : BaseUdpClient, IDisposable
    {
        /// <summary>
        /// 在控制台显示接收记录.[default]=true, set at ctor.
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

        /// <summary>
        /// 接收到Udp消息事件的处理器
        /// </summary>
        public event ReceiveEvent Received;

        private UdpClient _UdpClient;

        static UdpListener() { LogEnabled = true; }

        /// <summary>
        /// 生成一个<see cref="UdpListener"/>实例
        /// </summary>
        /// <param name="ip">远程Ip地址.[default]=null, 接收所有Ip地址发来的消息</param>
        /// <param name="port">远处端口号.[default]=0, 接收所有端口消息</param>
        public UdpListener(string ip = null, uint port = 11000) : base(ip, port) { }

        /// <summary>
        /// Udp监听进程
        /// </summary>
        private Thread _ListenThread;

        /// <summary>
        /// Udp start listening as background thread...
        /// </summary>
        public override void Open()
        {
            if (!CanOpen) return;
            _IpEndPointRemote = new IPEndPoint(_IpAddressRemote, (int)PortRemote);

            _UdpClient = new UdpClient(_IpEndPointRemote);
            _ListenThread = new Thread(StartLisening);
            _ListenThread?.Start();
        }

        public override void Close()
        {
            IsOpen = false;
            _ListenThread?.Abort();
            _UdpClient?.Close(); 
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        private void StartLisening()
        {
            try
            {
                IsOpen = true;
                if (LogEnabled)
                {
                    Logger.Info($"Local ip [{IpLocal}]");
                    Logger.Info($"Udp start listening to [{_IpEndPointRemote.Address}:{_IpEndPointRemote.Port}]");
                }
                do
                {
                    Buffer = _UdpClient.Receive(ref _IpEndPointRemote);
                    if (LogEnabled)
                    {
                        Logger.Info($"Udp received hex[{Buffer.Length}] from [{_IpEndPointRemote.Address}: {_IpEndPointRemote.Port}]: {Buffer.ToHexString(' ')}");
                        Logger.Info($"Udp received msg[{Buffer.Length}] from [{_IpEndPointRemote.Address}: {_IpEndPointRemote.Port}]: {Encoding.UTF8.GetString(Buffer)}");
                    }
                    Received?.Invoke(this, new ReceiveEventArgs(Buffer, _IpEndPointRemote));
                } while (IsOpen);
            }
            catch (Exception ex)
            {
                if (LogEnabled)
                {
                    Logger.Erro($"Udp listener open error: {ex.Message}"); 
                }
                throw ex;
            }
            finally
            {
                Close();
            }
        }

        #region GC
        private bool _Diposed = false; 
        ~UdpListener()
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
