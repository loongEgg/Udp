using LoongEgg.Core;
using System.Windows;

namespace LoongEgg.Udp.WpfDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IoC.Container.AddOrUpdate(MainViewModel.Singleton);
            MainView mainView = new MainView() { DataContext = IoC.Container.Get<MainViewModel>() }; 

            this.MainWindow = mainView;
            this.MainWindow.Show();
        }
    }
}
