using System.ComponentModel.DataAnnotations;

namespace InternRegister.Controllers.Interns.Requests;

public class CreateUpdateInternRequest
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }

    public required bool IsMale { get; set; }
    
    [EmailAddress]
    public required string Email { get; set; }
    
    [Phone]
    public string? Phone { get; set; }
    
    [Timestamp]
    public required long BirthDateTimeStamp { get; set; }
    
    public Guid? ProbationCourseId { get; set; }
    
    public Guid? ProbationProjectId { get; set; }
}