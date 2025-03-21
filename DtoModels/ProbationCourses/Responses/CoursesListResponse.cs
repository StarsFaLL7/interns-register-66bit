namespace InternRegister.Controllers.ProbationCourses.Responses;

public class CoursesListResponse
{
    public required CourseResponse[] Courses { get; set; }
    
    public required int WithoutPagingCount { get; set; }
}