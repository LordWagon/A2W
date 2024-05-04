using System.ComponentModel;
using System.Windows.Input;
using LoginManager.Shared;
using LoginManager;

public class LoginViewModel : BaseViewModel, INotifyPropertyChanged
{

    private string mainPassword;
    public string MainPassword
    {
        get => mainPassword;
        set
        {
            if (mainPassword != value)
            {
                mainPassword = value;
                OnPropertyChanged(nameof(MainPassword));
            }
        }
    }

    private bool incorrectPassword;
    public bool IncorrectPassword
    {
        get => incorrectPassword;
        set
        {
            if (incorrectPassword != value)
            {
                incorrectPassword = value;
                OnPropertyChanged(nameof(IncorrectPassword));
            }
        }
    }

    private string warningText;
    public string WarningText
    {
        get => warningText;
        set
        {
            if (warningText != value)
            {
                warningText = value;
                OnPropertyChanged(nameof(WarningText));
            }
        }
    }



    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
    public ICommand LoginCommand { get; }

    public LoginViewModel()
    {
        Setting.GetAll();
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;


        string colorSchemeFromDb = Setting.Get("ColorScheme");
        Setting.dateFormat = Setting.Get("DateFormat");
        if (colorSchemeFromDb != null && colorSchemeFromDb.Trim().Length > 0 && colorSchemeFromDb == "Dark")
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Dark;
        else
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Light;
        if (Setting.mainPassword != null && Setting.mainPassword.Length == 0)
            Application.Current.MainPage.Navigation.PushAsync(new ListPage()); 
        //MainPassword = Setting.mainPassword;
        //if (MainPassword == null)
        //{
        //    WarningText = "Chyba uložení hesla v databázi";
        //    IncorrectPassword = true;
        //}
       // LoginCommand = new Command(async () => await Login());
    }

    
    public async Task Login()
    {
        IncorrectPassword = false;
        WarningText = "";
        
            List<Setting> settings = Setting.GetAll();
        
        if (settings == null || settings.Count == 0 || settings.Exists(x => x.Key == "MainPassword") == false)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ListPage());
                return;
            }
        string _tempPassword = Setting.Get("MainPassword");
        if (_tempPassword == null)
        {
            WarningText = "Chyba uložení hesla v databázi";
            IncorrectPassword = true;
            return;
        }
        else if (_tempPassword != MainPassword)
        {
            WarningText = "Nesprávné heslo";
            IncorrectPassword = true;
            return;
        }
        else if (_tempPassword == Setting.EMTPY_MAIN_PASSWORD || _tempPassword == MainPassword)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ListPage());
            return;
        }
        else return;

//<<<<<<< Updated upstream
        
//        await _navigationService.NavigateToAsync<MainPageViewModel>();
//=======

        
//>>>>>>> Stashed changes
    }
    
}