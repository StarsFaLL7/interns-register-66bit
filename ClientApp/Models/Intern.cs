using System.ComponentModel.DataAnnotations;
using ClientApp.Components.TableView;

namespace ClientApp.Models;

public class Intern
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";
    
    public bool IsMale { get; set; }
    
    [EmailAddress]
    [DisplayInTableHeader("Email", 20, 100, 3, false)]
    public string Email { get; set; } = "";
    
    [Phone]
    [DisplayInTableHeader("Телефон", 15, 75, 4, false)]
    public string? Phone { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    [DisplayInTableHeader("Дата рождения", 15, 75, 2, false)]
    public string BirthDateStr => BirthDate.ToString("dd.MM.yyyy");
    
    public BriefCourseInfo? ProbationCourse { get; set; } = null!;
    
    public BriefProjectInfo? ProbationProject { get; set; } = null!;

    [DisplayInTableHeader("ФИ", 15, 75, 1, false)]
    public string FullName => LastName + " " + FirstName;

    [DisplayInTableHeader("Проект", 20, 100, 6, false)]
    public string ProjectTitle => ProbationProject?.Title ?? "-";
    
    [DisplayInTableHeader("Направление", 15, 100, 5, false)]
    public string CourseTitle => ProbationCourse?.Title ?? "-";
}