using LoginManager.Shared;

namespace LoginManager;

public partial class DetailPage : ContentPage
{
    public DetailPage(IDataStorageService storageService, int id)
    {
        InitializeComponent();
        this.BindingContext = new DetailViewModel(storageService, id);
    }
    
    private async void OnNavigateButtonBack(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}