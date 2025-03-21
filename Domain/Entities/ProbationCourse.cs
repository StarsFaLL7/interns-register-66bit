using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;

namespace Domain.Entities;

public class ProbationCourse : IHasId
{
    [Key]
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? Description { get; set; }

    public ICollection<Intern>? Interns { get; set; } = null!;
    
    public int InternsCount => Interns?.Count ?? 0;
}