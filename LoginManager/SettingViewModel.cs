using System.ComponentModel;
using System.Windows.Input;
using LoginManager.Shared;
using LoginManager;
using System.Collections.ObjectModel;

public class SettingViewModel : BaseViewModel, INotifyPropertyChanged
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

    public ObservableCollection<string> DateFormats { get; set; }
    public ObservableCollection<string> ColorSchemes { get; set; } 


    private string _selectedFormat;
    public string SelectedFormat
    {
        get => _selectedFormat;
        set
        {
            _selectedFormat = value;
            OnPropertyChanged();
        }
    }

    private string _selectedScheme;
    public string SelectedScheme
    {
        get => _selectedScheme;
        set
        {
            _selectedScheme = value;
            OnPropertyChanged();
            if (value != null && value.Trim().Length > 0 && value == "Dark")
                Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Dark;
            else
                Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Light;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ICommand SaveCommand { get; private set; }
    public ICommand ExportCommand { get; private set; }
    public ICommand ImportCommand { get; private set; }
    public ICommand NavigateBackCommand { get; }

    public delegate void ShowErrorMessage(string message);
    public event ShowErrorMessage OnShowErrorMessage;
    public void NotifyMessage(string errorMessage)
    {
        OnShowErrorMessage?.Invoke(errorMessage);
    }
    

    public SettingViewModel()
    {
        DateFormats = new ObservableCollection<string>()
        {
            "dd.MM.yyyy",
            "dd/MM/yyyy",
            "MM/dd/yyyy",
            "yyyy/MM/dd",
            "dd-MM-yyyy",
            "MM-dd-yyyy",
            "yyyy-MM-dd"
        };
        ColorSchemes = new ObservableCollection<string>() 
        {
            "Light",
            "Dark"
        };
        MainPassword = Setting.Get("MainPassword");
        SelectedFormat = Setting.Get("DateFormat");
        SelectedScheme = Setting.Get("ColorScheme");

        NavigateBackCommand = new Command(async () =>
            {
            string colorSchemeFromDb = Setting.Get("ColorScheme");
            if (colorSchemeFromDb != null && colorSchemeFromDb.Trim().Length > 0 && colorSchemeFromDb == "Dark")
                Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Dark;
            else
                Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Light;
            await Application.Current.MainPage.Navigation.PushAsync(new ListPage());
            }
        );

        SaveCommand = new Command(SaveSetting);
        ExportCommand = new Command(Export);
        ImportCommand = new Command(Import);
    }

    private async void SaveSetting()
    {
        List<Setting> settings = new List<Setting>();
        settings.Add(new Setting("MainPassword", MainPassword));
        settings.Add(new Setting("DateFormat", SelectedFormat));
        settings.Add(new Setting("ColorScheme", SelectedScheme));

        if (SelectedScheme != null && SelectedScheme.Trim().Length > 0 && SelectedScheme == "Dark")
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Dark;
        else
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Light;
        
        foreach (var setting in settings)
        {
            Setting.Create(setting);
        }
        NotifyMessage("Nastavení uloženo.");

        await Application.Current.MainPage.Navigation.PushAsync(new ListPage());

    }

    public DateTime? GetDateOfModifiedFileOnCloud() {
        try
        {
            XmlUtil xmlUtil = new XmlUtil();
            FtpSetting? ftpSetting = xmlUtil.GetSetting();
            if (ftpSetting == null)
            {
                Console.WriteLine("Failed to get setting");
                return null;
            }
            FtpUtil ftpUtil = new FtpUtil("ftp://" + ftpSetting.Hostname, ftpSetting.Username, ftpSetting.Password);
            return ftpUtil.GetLastModifiedTimestamp(ftpSetting.Folder + "//A2W.db");
        }
        catch (Exception e)
        {
            NotifyMessage("Vypadá to, že nejste připojen(a) ke cloudu. Zkontrolujte připojení na internet.");
            return null;
        }
    }

    private void Export()
    {
        try
        {
            XmlUtil xmlUtil = new XmlUtil();
            FtpSetting? ftpSetting = xmlUtil.GetSetting();
            if (ftpSetting == null)
            {
                Console.WriteLine("Failed to get setting");
                return;
            }
            FtpUtil ftpUtil = new FtpUtil("ftp://" + ftpSetting.Hostname, ftpSetting.Username, ftpSetting.Password);
            string dataPath = PlatformSpecifics.GetPathToDb();

            ftpUtil.UploadFile(dataPath, ftpSetting.Folder + "//A2W.db");
        }
        catch (Exception e)
        {
           NotifyMessage("Vypadá to, že nejste připojen(a) ke cloudu. Zkontrolujte připojení na internet.");
        }
    }

    private void Import() 
    {
        try 
        {
            XmlUtil xmlUtil = new XmlUtil();
            FtpSetting? ftpSetting = xmlUtil.GetSetting();
            if (ftpSetting == null)
            {
                Console.WriteLine("Failed to get setting");
                return;
            }

            FtpUtil ftpUtil = new FtpUtil("ftp://" + ftpSetting.Hostname, ftpSetting.Username, ftpSetting.Password);
            string oldDiskFilename = Path.Combine(PlatformSpecifics.GetDirectory(), "A2W.db");
            string newDownloadedFilename = Path.Combine(PlatformSpecifics.GetDirectory(), "A2W.db2");

            ftpUtil.DownloadFile(ftpSetting.Folder + "//A2W.db", newDownloadedFilename);

            //DateTime lastModified = File.GetLastWriteTime(oldDiskFilename);
            //DateTime uploadedToFtpDate = ftpUtil.GetLastModifiedTimestamp(ftpSetting.Folder + "//A2W.db");
            //if (uploadedToFtpDate > lastModified)
            //{
                File.Move(newDownloadedFilename, oldDiskFilename, overwrite: true);
            //}
        }
        catch (Exception e)
        {
            NotifyMessage("Vypadá to, že nejste připojen(a) ke cloudu. Zkontrolujte připojení na internet.");
        }
    }
}