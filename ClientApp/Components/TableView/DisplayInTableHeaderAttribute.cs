namespace ClientApp.Components.TableView;

[AttributeUsage(AttributeTargets.Property)]
public class DisplayInTableHeaderAttribute : Attribute
{
    public DisplayInTableHeaderAttribute(string name, int percentWidth, int minWidthPx, int maxWidthPx = -1, int order = 1)
    {
        DisplayName = name;
        PercentWidth = percentWidth;
        MinWidthPx = minWidthPx;
        MaxWidthPx = maxWidthPx;
        Order = order;
    }

    public DisplayInTableHeaderAttribute(string name, int percentWidth, int minWidthPx, int order = 1, bool htmlCode = false)
    {
        DisplayName = name;
        PercentWidth = percentWidth;
        MinWidthPx = minWidthPx;
        MaxWidthPx = int.MaxValue;
        Order = order;
        IsHtmlCode = htmlCode;
    }

    public string DisplayName { get; }
    public int PercentWidth { get; }
    public int MinWidthPx { get; }
    public int MaxWidthPx { get; }
    public int Order { get; }
    
    public bool IsHtmlCode { get; }
}