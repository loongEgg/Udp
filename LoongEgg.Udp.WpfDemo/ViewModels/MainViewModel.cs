using LoongEgg.Core;
using System.Windows.Input;

namespace LoongEgg.Udp.WpfDemo
{
    public class MainViewModel : BindableObject
    {
        public static MainViewModel Singleton
        {
            get
            {
                if (_Singleton == null)
                {
                    _Singleton = new MainViewModel();
                    IoC.Container.AddOrUpdate(_Singleton);
                }
                return _Singleton;
            }
        }
        private static MainViewModel _Singleton;

        public UdpListener Listener { get; set; }
        public UdpSender Sender { get; set; }

        public string MessageToSend
        {
            get { return _MessageToSend; }
            set { SetProperty(ref _MessageToSend, value); }
        }
        private string _MessageToSend = string.Empty;

        public string MessageReceived
        {
            get { return _MessageReceived; }
            set { SetProperty(ref _MessageReceived, value); }
        }
        private string _MessageReceived = string.Empty;

        public ICommand SenderSendCommand { get; }
        public ICommand SenderOpenOrCloseCommand { get; }
        public ICommand ListenerOpenOrCloseCommand { get; }

        MainViewModel()
        {
            Listener = new UdpListener();
            Sender = new UdpSender();

            Listener.Received += (s, e) => MessageReceived = $"{e.Ip}: {e.Port} > {e.Message}";

            SenderSendCommand = new DelegateCommand(() => Sender?.Send(MessageToSend), () => Sender != null && Sender.IsOpen);
            SenderOpenOrCloseCommand = new DelegateCommand(
                () =>
                {
                    if (Sender == null) return;
                    if (Sender.IsOpen)
                        Sender.Close();
                    else
                        Sender.Open();

                    (SenderSendCommand as DelegateCommand).RaiseCanExecuteChanged();
                },
                () =>
                {
                    if (Sender == null) return false;
                    if (Sender.IsOpen)
                    {
                        return true;
                    }
                    else
                    {
                        return Sender.CanOpen;
                    }
                });
            ListenerOpenOrCloseCommand = new DelegateCommand(
                () =>
                {
                    if (Listener == null) return;
                    if (Listener.IsOpen)
                        Listener.Close();
                    else
                        Listener.Open();
                    (ListenerOpenOrCloseCommand as DelegateCommand).RaiseCanExecuteChanged();
                },
                () =>
                {
                    if (Listener == null) return false;
                    if (Listener.IsOpen)
                    {
                        return true;
                    }
                    else
                    {
                        return Listener.CanOpen;
                    }
                });
        }
    }
}
