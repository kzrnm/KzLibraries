using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Windows;

namespace sandbox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Ioc.Default.ConfigureServices(new AppProvider());
        }

        private class AppProvider : IServiceProvider
        {
            public object? GetService(Type serviceType)
            {
                if (serviceType == typeof(MainViewModel))
                    return new MainViewModel();
                return null;
            }
        }
    }
}
