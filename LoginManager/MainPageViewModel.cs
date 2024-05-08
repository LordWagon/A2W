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
        get { return _items; }
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
    
    public bool isLoading = true;

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

    public delegate void Slide();
    public event Slide OnLoadingToSlide;
    public void NotifyBackToCodeBehind()
    {
        OnLoadingToSlide?.Invoke();
    }

    public MainPageViewModel(IDataStorageService storageService)
    {
        AllRecords = new List<Record>();
        Items = new SortableObservableCollection<Record>();
        
        dataService = storageService;

        Direction0 = _UP;
        Direction1 = _UP;
        Direction2 = _UP;

        LabelTappedCommand = new Command(async(object? arg) => await OnLabelTapped(arg));
        SettingCommand = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new SettingPage()));
        AddCommand = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new NewRecordPage()));
        FilterCommand = new Command(async () => await FilterChanged());

        LoadAll();
    }
    
    private async void LoadAll()
    {
        isLoading= true;
        foreach (var item in dataService.GetData())
        {
            Items.Add(item);
            AllRecords.Add(item);
            await Task.Delay(50);
        }
        isLoading = false;
        NotifyBackToCodeBehind();
    }

    private async Task OnLabelTapped(object? arg)
    {
        if (isLoading)
            return;
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
        if (isLoading)
            return;
        Items.SetRange(AllRecords.Where(x => x.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase)));
    }
    
    private string SwitchDirection(string recentDirection)
    {
        return recentDirection == _UP ? _DOWN : _UP;
    }
}
