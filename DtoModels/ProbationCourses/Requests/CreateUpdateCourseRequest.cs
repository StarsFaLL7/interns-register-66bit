namespace InternRegister.Controllers.ProbationCourses.Requests;

public class CreateUpdateCourseRequest
{
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public required Guid[] InternIds { get; set; }
}