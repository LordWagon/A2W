using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using LoginManager.Shared;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using System.ComponentModel.Design;
using Microsoft.Maui.ApplicationModel.Communication;

namespace LoginManager
{
    public partial class ListPage : ContentPage, INotifyPropertyChanged
    {
        public TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();

        /*
         * 
         * Na první stránce menu s nastavením
         *              - možnost nastavit heslo (volitelně)
         *                  - pokud bude nastavené, tak se při spuštění aplikace zobrazí dialog na zadání hesla
         *                  - pokud nebude nastavené, tak se úvodní obrazovka s loginem přeskočí až na hlavní 
         *              - možnost nastavit formát data
         *              - (barvy, velikost)
         */

        DataService dataService;

        public ListPage()
        {
            InitializeComponent();
            dataService = new DataService();
            this.BindingContext = new MainPageViewModel(dataService);

         
        }
        
        public void OnLeftClickEvent(object sender, EventArgs e) 
        {
            if (!(e is TappedEventArgs tappedEventArgs))
                return;

            if (!(tappedEventArgs.Parameter is int id))
                return;
            
            Navigation.PushAsync(new DetailPage(dataService, id));
        }

        public async void OnRightClickEvent(object sender, EventArgs e)
        {
            if (!(e is TappedEventArgs tappedEventArgs))
                return;

            if (!(tappedEventArgs.Parameter is int id))
                return;

            string name = dataService.Get(id).Name;
            bool answer = await DisplayAlert("Potvrzení", "Skutečně chcete smazat položku " + name, "Ano", "Ne").ConfigureAwait(true);

            if (answer)
            {
                dataService.Delete(id);
                this.BindingContext = new MainPageViewModel(dataService);
                
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.InvokeOnMainThreadAsync(async () =>
            {
                bool toSave = await DisplayAlert("Uložit?", "Chcete exportovat údaje i na cloud?", "Ano", "Ne");
                if (toSave)
                {
                    SettingViewModel vm = new SettingViewModel();

                    DateTime? modificationOnCloud = vm.GetDateOfModifiedFileOnCloud();
                    bool answer = false;
                    if (!modificationOnCloud.HasValue)
                    {
                        answer = await DisplayAlert("Potvrzení", "Export nahradí zálohované záznamy na cloudu. Opravdu checete exportovat data?", "Ano", "Ne");
                    }
                    else
                    {
                        answer = await DisplayAlert("Potvrzení", "Export nahradí zálohované záznamy na cloudu - ze dne: " + modificationOnCloud.Value.ToShortTimeString() + " " + modificationOnCloud.Value.ToShortDateString() + ". Opravdu checete exportovat data?", "Ano", "Ne");
                    }
                    if (answer)
                    {
                        vm.SaveCommand.Execute(null);
                        vm.ExportCommand.Execute(null);
                    }
                }
                System.Environment.Exit(0);
            });
            return true;
        }
    }
}