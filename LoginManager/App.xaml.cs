using System.ComponentModel;
using System.Windows.Input;
using LoginManager.Shared;
using LoginManager;
using Application = Microsoft.Maui.Controls.Application;

namespace LoginManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new MainPage());
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Light;
            MainPage = new AppShell();
        }
    }
}
