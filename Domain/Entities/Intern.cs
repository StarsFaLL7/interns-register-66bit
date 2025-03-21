using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Interfaces;

namespace Domain.Entities;

public class Intern : IHasId
{
    [Key]
    public required Guid Id { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }

    public required bool IsMale { get; set; }
    
    [EmailAddress]
    public required string Email { get; set; }
    
    [Phone]
    public string? Phone { get; set; }
    
    public required DateOnly BirthDate { get; set; }
    
    public Guid? ProbationCourseId { get; set; }
    [ForeignKey("ProbationCourseId")]
    public ProbationCourse? ProbationCourse { get; set; } = null!;
    
    public Guid? ProbationProjectId { get; set; }
    [ForeignKey("ProbationProjectId")]
    public ProbationProject? ProbationProject { get; set; } = null!;
}