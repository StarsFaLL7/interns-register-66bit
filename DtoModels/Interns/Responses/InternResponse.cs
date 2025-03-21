namespace InternRegister.Controllers.Interns.Responses;

public class InternResponse
{
    public required Guid Id { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }

    public required bool IsMale { get; set; }
    
    public required string Email { get; set; }
    
    public string? Phone { get; set; }
    
    public required DateOnly BirthDate { get; set; }
    
    public Guid? ProbationCourseId { get; set; }
    
    public string? ProbationCourseTitle { get; set; } = null!;
    
    public Guid? ProbationProjectId { get; set; }
    
    public string? ProbationProjectTitle { get; set; } = null!;
}