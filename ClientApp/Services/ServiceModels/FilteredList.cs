namespace ClientApp.Services.ServiceModels;

public class FilteredList<T> where T : class
{
    public required List<T> Items { get; set; }
    
    public required int WithoutPagingCount { get; set; }
}