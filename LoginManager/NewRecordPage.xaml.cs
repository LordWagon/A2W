namespace LoginManager;

public partial class NewRecordPage : ContentPage
{
    public NewRecordPage()
	{
        InitializeComponent();
        this.BindingContext = new NewRecordViewModel(new Shared.DataService());
    }
}