using System.Linq.Expressions;

namespace ClientApp.Components.TableView;

public class TableSortingOption<T>
{
    public required string Title { get; set; }
    
    public required bool ByDescending { get; set; }
    
    public required string PropertyName { get; set; }

    public override string ToString()
    {
        return Title;
    }
}