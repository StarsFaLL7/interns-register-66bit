using System.Text;
using ClientApp.Components.TableView;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Models;

public class ProbationCourse
{
    public Guid Id { get; set; }

    [DisplayInTableHeader("Название", 20, 200, 1, false)]
    public string Title { get; set; } = "";
    
    public string? Description { get; set; }
    
    public ICollection<Intern>? Interns { get; set; }

    [DisplayInTableHeader("Кол-во стажеров", 15, 100, 2, false)]
    public int InternsCount => Interns?.Count ?? 0;
    
    [DisplayInTableHeader("Описание", 45, 150, 3, false)]
    public string DescriptionSubString
    {
        get
        {
            var text = Description ?? "";
            return text.Length > 50 ? $"{text[..50]}..." : text;
        }
    }
    
    [DisplayInTableHeader("Стажеры", 20, 150, 3, true)]
    public MarkupString InternsDropdown
    {
        get
        {
            var options = new StringBuilder();
            foreach (var intern in (Interns ?? []).OrderBy(i => i.LastName))
            {
                options.Append($"<option value=\"{Guid.NewGuid()}\">{intern.LastName} {intern.FirstName}</option>");
            }
            return (MarkupString)$"""
                                  <select class="form-select">
                                  {options}
                                  </select>
                                  """;
        }
    }
    
}