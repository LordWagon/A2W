using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

public class SortableObservableCollection<T> : ObservableCollection<T>
{
    private bool suppressNotification = false;

    public SortableObservableCollection() : base() { }
    public SortableObservableCollection(IEnumerable<T> collection) : base(collection) { }

    public void SetRange(IEnumerable<T> list)
    {
        suppressNotification = true;
        this.Clear();

        foreach (var item in list)
        {
            Add(item);
        }

        suppressNotification = false;
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (!suppressNotification)
            base.OnCollectionChanged(e);
    }

    public void Sort<TKey>(Func<T, TKey> keySelector, bool ascending = true)
    {
        suppressNotification = true;

        List<T> sortedItems = ascending ? this.OrderBy(keySelector).ToList() : this.OrderByDescending(keySelector).ToList();
        for (int i = 0; i < sortedItems.Count; i++)
        {
            this[i] = sortedItems[i];
        }

        suppressNotification = false;
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}
