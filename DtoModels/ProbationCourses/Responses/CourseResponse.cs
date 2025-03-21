using InternRegister.Controllers.Interns.Responses;

namespace InternRegister.Controllers.ProbationCourses.Responses;

public class CourseResponse
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public required InternResponse[] Interns { get; set; }
}