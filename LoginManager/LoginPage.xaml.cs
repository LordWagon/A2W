using LoginManager.Shared;

namespace LoginManager;

public partial class LoginPage : ContentPage
{
    public LoginPage()
	{
		InitializeComponent();
        this.BindingContext = new LoginViewModel();
    }

    private async void OnNavigateButtonLoginClicked(object sender, EventArgs e)
    {
        if (MainPassword.Text != Setting.mainPassword)
        {
            DisplayAlert("Chyba", "Špatné heslo", "OK");
            return;
        }
        var navigationPage = new NavigationPage(new ListPage());
        //var navigationPage = new NavigationPage(new SettingPage());
        await Navigation.PushAsync(navigationPage);
    }
}