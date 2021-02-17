using LoongEgg.Core;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace LoongEgg.Udp
{
    /// <summary>
    /// Udp服务的基类
    /// </summary>
    public abstract class BaseUdpClient : BindableObject
    {

        /// <summary>
        /// IP地址, 接收器置null时监听所有IP地址.IP设置为"127.0.0.1"为回环模式, 即本机监听本机用于调试
        /// </summary>
        public string IpRemote
        {
            get { return _IpRemote; }
            set
            {
                if (IsOpen) return;

                if (string.IsNullOrEmpty(value))
                {
                    _IpAddressRemote = IPAddress.Any;
                    SetProperty(ref _IpRemote, IPAddress.Any.ToString());
                }
                else
                {
                    IPAddress address;
                    if (IPAddress.TryParse(value, out address))
                    {
                        _IpAddressRemote = address;
                        SetProperty(ref _IpRemote, address.ToString());
                    }
                    else
                    {
                        _IpAddressRemote = null;
                        SetProperty(ref _IpRemote, value);
                    }
                }
                RaisePropertyChanged(nameof(CanOpen));
            }
        }
        protected string _IpRemote = string.Empty;

        /// <summary>
        /// 可以打开标记
        /// </summary>
        public bool CanOpen => _IpAddressRemote != null && !IsOpen;

        /// <summary>
        /// 远程端口号
        /// </summary>
        public uint PortRemote
        {
            get { return _PortRemote; }
            set
            {
                if (IsOpen) return;
                SetProperty(ref _PortRemote, value);
            }
        }
        protected uint _PortRemote;

        /// <summary>
        /// 本地Ip
        /// </summary>
        public string IpLocal
        {
            get { return _IpLocal; }
            protected set { SetProperty(ref _IpLocal, value); }
        }
        protected string _IpLocal;

        /// <summary>
        /// 打开标志
        /// </summary>
        public bool IsOpen
        {
            get { return _IsOpen; }
            protected set { SetProperty(ref _IsOpen, value); }
        }
        protected bool _IsOpen;

        /// <summary>
        /// 当前接收/待发送的缓冲
        /// </summary>
        public byte[] Buffer
        {
            get { return _Buffer; }
            set { SetProperty(ref _Buffer, value); }
        }
        private byte[] _Buffer;

        /// <summary>
        /// 远端Ip地址
        /// </summary>
        protected IPAddress _IpAddressRemote;
        /// <summary>
        /// 远程端口
        /// </summary>
        protected IPEndPoint _IpEndPointRemote;

        /// <summary>
        /// 生成一个新的Udp接收/发送服务器
        /// </summary>
        /// <param name="ip">目标Ip地址</param>
        /// <param name="port">目标端口号</param>
        public BaseUdpClient(string ip, uint port)
        {
            IpRemote = ip;
            PortRemote = port;

            var hostName = Dns.GetHostName();
            var address = Dns.GetHostEntry(hostName).AddressList.Where(
                a => a.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            IpLocal = address.ToString();
        }

        /// <summary>
        /// 打开接收/发送功能
        /// </summary>
        public abstract void Open();

        /// <summary>
        /// 关闭接收/发送功能
        /// </summary>
        public abstract void Close();
    }
}
