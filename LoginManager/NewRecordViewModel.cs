﻿using System.ComponentModel;
using System.Windows.Input;
using LoginManager.Shared;
using LoginManager; // Assuming this namespace contains your data models

public class NewRecordViewModel : BaseViewModel, INotifyPropertyChanged
{
    Record record = new Record();
    IDataStorageService _dataService;

    private string name;
    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    private string location;
    public string Location
    {
        get => location;
        set
        {
            if (location != value)
            {
                location = value;
                OnPropertyChanged(nameof(Location));
            }
        }
    }

    private string username;
    public string Username
    {
        get => username;
        set
        {
            if (username != value)
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
    }

    private string password;
    public string Password
    {
        get => password;
        set
        {
            if (password != value)
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
    }

    private string notes;
    public string Notes
    {
        get => notes;
        set
        {
            if (notes != value)
            {
                notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ICommand SaveCommand { get; private set; }

    
    public ICommand NavigateBackCommand { get; }

    public NewRecordViewModel(IDataStorageService storageService)
    {
        _dataService = storageService;

        NavigateBackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new ListPage()));
        SaveCommand = new Command(SaveRecord);
    }

    private void SaveRecord()
    {
        record.Name = Name;
        record.Location = Location;
        record.Username = Username;
        record.Password = Password;
        record.Notes = Notes;
        record.DateOnlyCreated = DateOnly.FromDateTime(DateTime.Now);
        record.DateCreated = record.MakeDateForDB();
        record.DateToDisplay = record.MakeDateToDisplay();
        _dataService.Create(record);

        NavigateBackCommand.Execute(null);
    }
}