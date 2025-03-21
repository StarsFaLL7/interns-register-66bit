namespace InternRegister.Controllers.ProbationProjects.Requests;

public class CreateUpdateProjectRequest
{
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public long StartDateTimestamp { get; set; }
    
    public required Guid[] InternIds { get; set; }
}