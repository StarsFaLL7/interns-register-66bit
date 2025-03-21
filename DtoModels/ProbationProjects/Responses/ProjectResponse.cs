using InternRegister.Controllers.Interns.Responses;

namespace InternRegister.Controllers.ProbationProjects.Responses;

public class ProjectResponse
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public required DateOnly StartDate { get; set; }
    
    public required InternResponse[] Interns { get; set; }
}