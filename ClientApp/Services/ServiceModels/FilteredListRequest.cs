namespace ClientApp.Services.ServiceModels;

public class FilteredListRequest
{
    public int Skip { get; set; }
    
    public int Take { get; set; }
    
    public string? OrderBy { get; set; }
    
    public bool Ascending { get; set; } 
    
    public string? Search { get; set; }

    public Dictionary<string, string> AdditionalQueryParams { get; set; } = [];
}