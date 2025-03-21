namespace InternRegister.Controllers.ProbationProjects.Responses;

public class ProjectsListResponse
{
    public required ProjectResponse[] Projects { get; set; }
    
    public required int WithoutPagingCount { get; set; }
}