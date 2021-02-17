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

        public ICommand SendCommand { get; }
        public ICommand OpenOrCloseCommand { get; }

        MainViewModel()
        {
            Listener = new UdpListener();
            Sender = new UdpSender();

            SendCommand = new DelegateCommand(() => Sender?.Send(Sender.Buffer), () => Sender != null && Sender.IsOpen);
            OpenOrCloseCommand = new DelegateCommand(
                () =>
                {
                    if (Sender == null) return;
                    if (Sender.IsOpen)
                        Sender.Close();
                    else
                        Sender.Open();

                    (SendCommand as DelegateCommand).RaiseCanExecuteChanged();
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

        }
    }
}
