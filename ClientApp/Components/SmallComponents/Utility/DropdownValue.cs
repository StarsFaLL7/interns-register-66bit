namespace ClientApp.Components.SmallComponents.Utility;

public class DropdownValue<T>
{
    public required T Value { get; set; }
    
    public Guid UniqueId { get; set; }
}