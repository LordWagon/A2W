using LoginManager.Shared;
using System.Threading;

namespace LoginManager;

public partial class SettingPage : ContentPage
{
    private CancellationTokenSource _cancellationTokenSourceExport;
    private CancellationTokenSource _cancellationTokenSourceImport;

    public SettingPage()
	{
		InitializeComponent();
        SettingViewModel svm = new SettingViewModel();
        this.BindingContext = svm;
        Subscribe(svm);
    }

    private async Task MadeChange(object? sender, EventArgs e, CancellationToken token)
    {
        for (int i = 0; i < 20; i++)
        {
            Button button = (Button)sender;
            if (await button.FadeTo((float)i/20, 100) == true)
                throw new TaskCanceledException(); 
            await Task.Delay(100, token);
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

        }
    }



    private async void OnExportPressed(object? sender, EventArgs e)
    {
        Button btn = (Button)sender;
        Button button = (Button)sender;
        _cancellationTokenSourceExport?.Cancel(); // Zruší předchozí časovač, pokud existuje
        _cancellationTokenSourceExport = new CancellationTokenSource();

        try
        {
            //            await button.FadeTo(0, 1000).ConfigureAwait(ConfigureAwaitOptions.None);
            //            await button.FadeTo(1, 1000);
            //            await Task.Delay(2000, _cancellationTokenSourceExport.Token);
//            if (tokenSource.IsCancellationRequested)
 //               throw new TaskCanceledException();

                        await MadeChange(button, e, _cancellationTokenSourceExport.Token); // Akce, která se má provést ihned
            //await Task.Delay(30000, _cancellationTokenSourceExport.Token); // Počká 10 vteřin
            // Akce, která se má provést po 10 vteřinách
            SettingViewModel vm = (SettingViewModel)this.BindingContext;
            
            DateTime? modificationOnCloud = vm.GetDateOfModifiedFileOnCloud();
            bool answer = false;
            if (!modificationOnCloud.HasValue)
            {
                answer = await DisplayAlert("Potvrzení", "Export nahradí zálohované záznamy na cloudu. Opravdu checete exportovat data?", "Ano", "Ne");
            } else
            {
                answer = await DisplayAlert("Potvrzení", "Export nahradí zálohované záznamy na cloudu - ze dne: " + modificationOnCloud.Value.ToShortTimeString() + " " + modificationOnCloud.Value.ToShortDateString()  + ". Opravdu checete exportovat data?", "Ano", "Ne");
            }
            if (answer)
            {
                vm.SaveCommand.Execute(null);
                vm.ExportCommand.Execute(null);
            }
        }
        catch (TaskCanceledException)
        {
            await button.FadeTo(1, 1);
            await DisplayAlert("Info", "Proces exportu byl zrušen. Pro správný export stiskněte tlačítko po dobu minimálně 2 vteřiny.", "Budiž");
            
        }
        finally
        {
            _cancellationTokenSourceExport?.Cancel();
        }

    }

    private async void OnExportReleased(object? sender, EventArgs e)
    {
        _cancellationTokenSourceExport?.Cancel(); // Zruší čekání, pokud uživatel uvolní tlačítko předčasně
    }


    private async void OnImportPressed(object? sender, EventArgs e)
    {
        Button button = (Button)sender;
        
        _cancellationTokenSourceImport?.Cancel(); // Zruší předchozí časovač, pokud existuje
        _cancellationTokenSourceImport = new CancellationTokenSource();

        try
        {
            await MadeChange(button, e, _cancellationTokenSourceImport.Token);
            SettingViewModel vm = (SettingViewModel)this.BindingContext;
            DateTime? modificationOnCloud = vm.GetDateOfModifiedFileOnCloud();
            bool answer = false;
            if (!modificationOnCloud.HasValue)
            {
                answer = await DisplayAlert("Potvrzení", "Import nahradí současné záznamy. Opravdu checete importovat data?", "Ano", "Ne");
            }
            else
            {
                answer = await DisplayAlert("Potvrzení", "Import nahradí současné záznamy. Opravdu checete importovat data ze dne " + modificationOnCloud.Value.ToShortTimeString() + " " + modificationOnCloud.Value.ToShortDateString() + "?", "Ano", "Ne");
            }
            if (answer)
            {
                vm.ImportCommand.Execute(null);
            }
        }
        catch (TaskCanceledException)
        {
            await button.FadeTo(1, 1);
            await DisplayAlert("Info", "Proces importu byl zrušen. Pro správný import stiskněte tlačítko po dobu minimálně 2 vteřiny.", "Budiž");
        }
        finally
        {
            _cancellationTokenSourceImport?.Cancel();
        }
    }

    private async void OnImportReleased(object? sender, EventArgs e)
    {
        _cancellationTokenSourceImport?.Cancel(); // Zruší čekání, pokud uživatel uvolní tlačítko předčasně
    }

    public void Subscribe(SettingViewModel svm) 
    {
        svm.OnShowErrorMessage += DisplayMessage; 
    }

    public void DisplayMessage(string message)
    {
        DisplayAlert("Info", message, "Budiž");
    }
}