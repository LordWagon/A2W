using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using LoginManager.Shared;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using System.ComponentModel.Design;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls.Compatibility;
using System.Threading;

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
        MainPageViewModel viewModel;
        Animation animation1;
        Animation animation0;
        double startMainBlockHeight = 0;
        double endMainBlockHeight = 0;
        bool isLoaded = false;
        
        public ListPage()
        {
            InitializeComponent();
            dataService = new DataService();
            viewModel = new MainPageViewModel(dataService);
            DataItems.ItemsSource = viewModel.Items;
            this.SizeChanged += ListPage_SizeChanged;    
            UpperGrid.SizeChanged += ListPage_SizeChanged;
            viewModel.IsSorting += DisableItems;



            WrapperGrid.RowDefinitions[0].Height = new GridLength(0);
            WrapperGrid.RowDefinitions[1].Height = new GridLength(startMainBlockHeight);
            UnderGrid.RowDefinitions[0].Height = new GridLength(startMainBlockHeight);
            WrapperGrid.RowDefinitions[2].Height = new GridLength(90);


            OnLoading();
            this.BindingContext = viewModel;
        }

        private void ListPage_SizeChanged(object? sender, EventArgs e)
        {
            startMainBlockHeight = 560 + (this.Window.Height - 854) + 40;
            endMainBlockHeight = 360 + (this.Window.Height - 854) + 40;

            if (!isLoaded)
            {
                UpperGrid.IsVisible = false;


                WrapperGrid.RowDefinitions[0].Height = new GridLength(0);
                WrapperGrid.RowDefinitions[1].Height = new GridLength(startMainBlockHeight);
                UnderGrid.RowDefinitions[0].Height = new GridLength(startMainBlockHeight);
                WrapperGrid.RowDefinitions[2].Height = new GridLength(90);


                //UpperGrid.RowDefinitions[0].Height = new GridLength(0);
                //UpperGrid.RowDefinitions[1].Height = new GridLength(0);
                //heightGap = 100;            
            }
            else
            {
                UpperGrid.IsVisible = true;
                WrapperGrid.RowDefinitions[0].Height = new GridLength(200);
                WrapperGrid.RowDefinitions[1].Height = new GridLength(endMainBlockHeight);
                UnderGrid.RowDefinitions[0].Height = new GridLength(endMainBlockHeight);
                WrapperGrid.RowDefinitions[2].Height = new GridLength(90);
            }

            //1023, 853.33

            /*
            WrapperGrid.RowDefinitions[2].Height = new GridLength(50);
            UnderGrid.RowDefinitions[0].Height = new GridLength(
                this.Window.Height - (
                    WrapperGrid.RowDefinitions[0].Height.Value +
                    WrapperGrid.RowDefinitions[2].Height.Value));
            WrapperGrid.RowDefinitions[1].Height = new GridLength(UnderGrid.RowDefinitions[0].Height.Value);
            */
            //WrapperGrid.RowDefinitions[2].Height = new GridLength(this.Window.Height - WrapperGrid.RowDefinitions[0].Height.Value - WrapperGrid.RowDefinitions[1].Height.Value);
                
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
                //this.BindingContext = new MainPageViewModel(dataService);
                await Navigation.PushAsync(new ListPage());
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

        public void OnLoading()
        {
                viewModel.OnLoadingToSlide += Slide;
                
        }

        public void Slide()
        {
            isLoaded = true;
            UpperGrid.IsVisible = true;
            Debug.WriteLine("Slide after loading");

            animation1 = new Animation(
                callback: v => MainThread.BeginInvokeOnMainThread(() => WrapperGrid.RowDefinitions[0].Height = new GridLength(v)),
                start: 0,
                end: 200
            );
            animation1.Commit(
                owner: UpperGrid,
                name: "RowHeightAnimation0",
                length: 200,
                easing: Easing.Linear,
                finished: (v, c) => MainThread.BeginInvokeOnMainThread(() => WrapperGrid.RowDefinitions[0].Height = new GridLength(200, GridUnitType.Absolute)),
                repeat: () => false
            );

            animation0 = new Animation(
                callback: v => MainThread.BeginInvokeOnMainThread(() => WrapperGrid.RowDefinitions[1].Height = new GridLength(v)),
                start: startMainBlockHeight,
                end: endMainBlockHeight
            );
            animation0.Commit(
                    owner: UpperGrid,
                    name: "RowHeightAnimation1",
                    length: 200,
                    easing: Easing.Linear,
                    finished: (v, c) => MainThread.BeginInvokeOnMainThread(() => WrapperGrid.RowDefinitions[1].Height = new GridLength(endMainBlockHeight, GridUnitType.Absolute)),
                    repeat: () => false
            );
            var animation0b = new Animation(
                callback: v => MainThread.BeginInvokeOnMainThread(() => UnderGrid.RowDefinitions[0].Height = new GridLength(v)),
                start: startMainBlockHeight,
                end: endMainBlockHeight
            );
            animation0b.Commit(
                    owner: UpperGrid,
                    name: "RowHeightAnimation1b",
                    length: 200,
                    easing: Easing.Linear,
                    finished: (v, c) => MainThread.BeginInvokeOnMainThread(() => UnderGrid.RowDefinitions[0].Height = new GridLength(endMainBlockHeight, GridUnitType.Absolute)),
                    repeat: () => false
            );
        }

        public void DisableItems(bool isSorting)
        {
            if ( isSorting )
                DataItems.IsEnabled = false;
            else
                DataItems.IsEnabled = true;
            Task.Delay(2000);
            //DataItems.IsEnabled = !isSorting;
        }
    }
}