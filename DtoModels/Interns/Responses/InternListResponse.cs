namespace InternRegister.Controllers.Interns.Responses;

public class InternListResponse
{
    public required InternResponse[] Interns { get; set; }
    
    public required int WithoutPagingCount { get; set; }
}