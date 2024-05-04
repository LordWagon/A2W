/*namespace LoginManager
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<Type, Type> _mappings; // Maps view models to views
        public NavigationService()
        {
            _mappings = new Dictionary<Type, Type>();
            // Register pages and their corresponding view models
            //Register<MainPageViewModel, MainPage>();
            Register<DetailViewModel, DetailPage>();
            Register<NewRecordViewModel, NewRecordPage>();
            Register<SettingViewModel, SettingPage>();
            Register<LoginViewModel, LoginPage>();
        }

        public async Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            Type pageType = _mappings[typeof(TViewModel)];
            Page page = Activator.CreateInstance(pageType) as Page;

            if (page != null)
                await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task GoBackAsync()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        protected void Register<TViewModel, TPage>()
        {
            _mappings[typeof(TViewModel)] = typeof(TPage);
        }
    }

}
*/
