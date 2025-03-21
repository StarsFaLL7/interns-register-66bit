using System.Reflection;
using ClientApp.Components.SmallComponents;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.TableView;

public partial class TableView<T>
{
    [Parameter] public bool AddEditButton { get; set; } = true;
    [Parameter] public bool AddDeleteButton { get; set; } = true;
    [Parameter] public Func<T, Task>? EditButtonClickedCallback { get; set; }
    [Parameter] public Func<T, Task>? DeleteButtonClickedCallback { get; set; }
    [Parameter] [EditorRequired] public Func<bool, Task> ReloadRequested { get; set; } = null!;
    [Parameter] [EditorRequired] public TableSortingOption<T>[] SortingOptions { get; set; } = [];
    
    private List<PropertyInfo> DisplayProperties { get; set; } = null!;
    private int _itemsPerPage = 10;
    private int _lastPage;
    private int _totalPagesCount;
    
    public T[] DisplayItemsList { get; private set; } = [];
    public int TotalItemsCount { get; private set; }
    public int CurrentPage { get; private set; }
    
    public TableSortingOption<T>? SelectedSortedOption { get; set; }
    public int ItemsPerPage
    {
        get => _itemsPerPage;
        set
        {
            var reloadRequested = _itemsPerPage != value;
            _itemsPerPage = value;
            if (reloadRequested)
            {
                ReloadRequested.Invoke(true);
            }
        }
    }

    protected override void OnInitialized()
    {
        GenerateDisplayProperties();

        ReloadState(true);
    }

    protected override Task OnParametersSetAsync()
    {
        if (SortingOptions.Length > 0)
        {
            SelectedSortedOption = SortingOptions[0];
        }
        return Task.CompletedTask;
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
        {
            _ddItemsPerPage?.SelectItem(ItemsPerPage);
            _ddSortingOption?.SelectItem(SelectedSortedOption);
        }
    }

    private void GenerateDisplayProperties()
    {
        DisplayProperties = new List<PropertyInfo>();
        var propertiesByAttribute = new Dictionary<DisplayInTableHeaderAttribute, PropertyInfo>();
        foreach (var property in typeof(T).GetProperties())
        {
            var attribute = GetAttributeOrDefault(property);
            if (attribute != null)
            {
                propertiesByAttribute[attribute] = property;
            }
        }

        DisplayProperties = propertiesByAttribute
            .OrderBy(p => p.Key.Order)
            .Select(pair => pair.Value)
            .ToList();
    }

    private DisplayInTableHeaderAttribute? GetAttributeOrDefault(PropertyInfo propertyInfo)
    {
        return propertyInfo.GetCustomAttribute<DisplayInTableHeaderAttribute>();
    }

    public void ReloadState(bool resetToFirstPage)
    {
        if (resetToFirstPage)
        {
            CurrentPage = 0;
        }
        StateHasChanged();
    }
    
    public void ReloadDisplayList(T[] displayItemsList, int totalItemsCount, bool resetToFirstPage)
    {
        if (resetToFirstPage)
        {
            CurrentPage = 0;
        }
        _totalPagesCount = totalItemsCount / ItemsPerPage + 
            (totalItemsCount % ItemsPerPage > 0 ? 1 : 0);
        DisplayItemsList = displayItemsList;
        TotalItemsCount = totalItemsCount;
        StateHasChanged();
    }

    private void EditButtonClicked(T obj)
    {
        EditButtonClickedCallback?.Invoke(obj);
    }

    private void DeleteButtonClicked(T obj)
    {
        DeleteButtonClickedCallback?.Invoke(obj);
    }

    private void ItemsPerPageDdValueChanged(int obj)
    {
        var reloadRequested = ItemsPerPage != obj;
        ItemsPerPage = obj;
        if (reloadRequested)
        {
            ReloadRequested.Invoke(true);
        }
    }
    
    #region Pagination
    
    private void GoToPageInputChanged(ChangeEventArgs obj)
    {
        var str = obj.Value!.ToString();
        if (string.IsNullOrWhiteSpace(str) || !int.TryParse(str, out var pageNum))
        {
            return;
        }
        if (pageNum > _totalPagesCount || pageNum < 1)
        {
            return;
        }
        CurrentPage = pageNum - 1;
        ReloadRequested?.Invoke(false);
    }
    
    public void SelectPage(int index)
    {
        if (index < 0 || index >= _totalPagesCount)
        {
            return;
        }
        CurrentPage = index;
        
        if (_lastPage != CurrentPage)
        {
            _lastPage = CurrentPage;
            ReloadRequested.Invoke(false);
        }
    }

    private void PreviousPage()
    {
        if (CurrentPage > 0)
        {
            CurrentPage -= 1;
        }
        if (_lastPage != CurrentPage)
        {
            _lastPage = CurrentPage;
            ReloadRequested.Invoke(false);
        }
    }

    private void NextPage()
    {
        if (CurrentPage < _totalPagesCount - 1)
        {
            CurrentPage += 1;
        }
        if (_lastPage != CurrentPage)
        {
            _lastPage = CurrentPage;
            ReloadRequested.Invoke(false);
        }
    }

    private void FirstPage()
    {
        CurrentPage = 0;
        if (_lastPage != CurrentPage)
        {
            _lastPage = CurrentPage;
            ReloadRequested.Invoke(false);
        }
    }

    private void LastPage()
    {
        CurrentPage = _totalPagesCount - 1;
        if (_lastPage != CurrentPage)
        {
            _lastPage = CurrentPage;
            ReloadRequested.Invoke(false);
        }
    }

    #endregion

    private void SortDropDownChanged(TableSortingOption<T>? obj)
    {
        var reloadRequested = SelectedSortedOption != obj;
        SelectedSortedOption = obj;
        if (reloadRequested)
        {
            ReloadRequested.Invoke(false);
        }
    }

    public void SelectSoringOptionByPropertyName(string propertyName, bool descending)
    {
        var sortOption = SortingOptions.FirstOrDefault(s => s.PropertyName == propertyName && s.ByDescending == descending);
        if (sortOption == null)
        {
            return;
        }
        SortDropDownChanged(sortOption);
    }
}