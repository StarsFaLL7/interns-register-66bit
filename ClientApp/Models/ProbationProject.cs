using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using ClientApp.Components.TableView;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Models;

public class ProbationProject
{
    public Guid Id { get; set; }

    [DisplayInTableHeader("Название", 15, 200, 1, false)]
    public string Title { get; set; } = "";
    
    public DateTime StartDate { get; set; }

    [DisplayInTableHeader("Дата начала", 10, 100, 2, false)]
    public string DateStr => StartDate.ToString("dd.MM.yyyy");
    public string? Description { get; set; }
    
    public ICollection<Intern>? Interns { get; set; }
    
    [DisplayInTableHeader("Кол-во стажеров", 10, 100, 3, false)]
    public int InternsCount => Interns?.Count ?? 0;
    
    [DisplayInTableHeader("Описание", 45, 150, 4, false)]
    public string DescriptionSubString
    {
        get
        {
            var text = Description ?? "";
            return text.Length > 50 ? $"{text[..50]}..." : text;
        }
    }
    
    [DisplayInTableHeader("Стажеры", 20, 150, 5, true)]
    public MarkupString InternsDropdown
    {
        get
        {
            var options = new StringBuilder();
            foreach (var intern in (Interns ?? []).OrderBy(i => i.LastName))
            {
                options.Append($"<option value=\"{Guid.NewGuid()}\">{WebUtility.HtmlEncode(intern.LastName)} {WebUtility.HtmlEncode(intern.FirstName)}</option>");
            }
            return (MarkupString)$"<select class=\"form-select\">{options}</select>";
        }
    }
}