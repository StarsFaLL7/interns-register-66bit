using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;

namespace Domain.Entities;

public class ProbationProject : IHasId
{
    [Key]
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required DateOnly StartDate { get; set; }
    
    public string? Description { get; set; }

    public ICollection<Intern>? Interns { get; set; } = null!;
}