using System.ComponentModel;
using System.Windows.Input;
using LoginManager.Shared;
using LoginManager;


public class MainPageViewModel : BaseViewModel, INotifyPropertyChanged
{
    readonly string _DOWN = Char.ToString((char)0x25BC);
    readonly string _UP = Char.ToString((char)0x25B2);

    private SortableObservableCollection<Record> _items;
    public SortableObservableCollection<Record> Items
    {
        get => _items;
        set
        {
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    private string _direction0;
    public string Direction0
    {
        get { return _direction0; }
        set
        {
            _direction0 = value;
            OnPropertyChanged(nameof(Direction0));
        }
    }

    private string _direction1;
    public string Direction1
    {
        get { return _direction1; }
        set
        {
            _direction1 = value;
            OnPropertyChanged(nameof(Direction1));
        }
    }

    private string _direction2;
    public string Direction2
    {
        get { return _direction2; }
        set
        {
            _direction2 = value;
            OnPropertyChanged(nameof(Direction2));
        }
    }
    
    private string _filterText;
    public string FilterText
    {
        get => _filterText;
        set
        {
            if (_filterText != value)
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
            }
        }
    }

    private List<Record> allRecords;
    public List<Record> AllRecords
    {
        get => allRecords;
        set => allRecords = value;
    }

    private IEnumerable<Record> allRecordsE;
    public IEnumerable<Record> AllRecordsE
    {
        get => allRecordsE;
        set => allRecordsE = value;
    }

    
    private int currentPage = 0;

#if ANDROID
    private int itemsPerPage = 20; 
#else
    private int itemsPerPage = 8192;
#endif
    public bool isLoading = false;
    

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ICommand FilterCommand { get; private set; }

    public ICommand SettingCommand { get; private set; }
    public ICommand LabelTappedCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand LoadMoreItemsCommand { get; }

    IDataStorageService dataService;

    public MainPageViewModel(IDataStorageService storageService)
    {
        Items = new SortableObservableCollection<Record>(); 
        AllRecords = new List<Record>();
        
        
        dataService = storageService;

        Direction0 = _UP;
        Direction1 = _UP;
        Direction2 = _UP;

        LabelTappedCommand = new Command(async(object? arg) => await OnLabelTapped(arg));
        SettingCommand = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new SettingPage()));
        AddCommand = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new NewRecordPage()));
        FilterCommand = new Command(async () => await FilterChanged());

        LoadMoreItemsCommand = new Command(LoadMoreItems);
        Task.Run(() => LoadInitialItems());

        //Task.Run(async () => await LoadAll());
        //LoadInitialItems();
    }
    
    private void LoadInitialItems()
    {
        LoadMoreItems();
    }
    
    /*
    private void LoadMoreItems()
    {
        if (isLoading)
            return;

        isLoading = true;

        var items = DataService.GetItems(currentPage, itemsPerPage);
        foreach (var item in items)
        {
            Items.Add(item);
        }

        currentPage++;
        isLoading = false;
    }
    */

    
    private void LoadMoreItems()
    {
        if (isLoading) return;
        isLoading = true;

        // Simulate an async data load
        Task.Run(() =>
        {
            var newItems = DataService.GetItems(currentPage, itemsPerPage);
            Device.BeginInvokeOnMainThread(() =>
            {
            //    Items.SetRange(newItems, false);
                foreach (var item in newItems)
                {
                    Items.Add(item);
                    AllRecords.Add(item);
                }
                currentPage++;
                isLoading = false;
            });
        });
    }
    

    /*
    private async Task LoadAll()
    {
        for (int currentPage = 0; currentPage < 20; currentPage++)
        {
            AllRecords.AddRange(DataService.GetItems(currentPage, itemsPerPage));
            Items.SetRange(DataService.GetItems(currentPage, itemsPerPage), false);
            //AllRecords.AddRange(DataService.GetItems(i, itemsPerPage));
            //Items.SetRange(AllRecords);
        }
    }
    */

    /*
    private async Task LoadItems()
    {
        //AllRecords = dataService.GetAll();
        
        
        IEnumerator<Record> ienum = dataService.GetEnumerator();
        while (ienum.MoveNext())
        {
            Items.Add(ienum.Current);
            AllRecords.Add(ienum.Current);
        }
    }
    */


    private async Task OnLabelTapped(object? arg)
    {
        switch (arg)
        {
            case "0":
                if (Direction0 == _DOWN)
                    Task.Run(() => Items.Sort(x => x.Id, true));
                else
                    Task.Run(() => Items.Sort(x => x.Id, false));
                Direction0 = SwitchDirection(Direction0);
                break;
            case "1":
                if (Direction1 == _DOWN)
                    Task.Run(() => Items.Sort(x => x.Name, true));
                else
                    Task.Run(() => Items.Sort(x => x.Name, false));
                Direction1 = SwitchDirection(Direction1);
                break;
            case "2":
                if (Direction2 == _DOWN)
                    Task.Run(() => Items.Sort(x => x.DateOnlyCreated, true));
                else
                    Task.Run(() => Items.Sort(x => x.DateOnlyCreated, false));
                Direction2 = SwitchDirection(Direction2);
                break;
        }
    }

    private async Task FilterChanged()
    {
        Items.SetRange(AllRecords.Where(x => x.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase)));
    }

    private string SwitchDirection(string recentDirection)
    {
        if (recentDirection == _UP)
        {
            return _DOWN;
        }
        else
        {
            return _UP;
        }
    }
}
