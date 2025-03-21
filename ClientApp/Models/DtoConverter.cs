using InternRegister.Controllers.Interns.Responses;
using InternRegister.Controllers.ProbationCourses.Responses;
using InternRegister.Controllers.ProbationProjects.Responses;

namespace ClientApp.Models;

public static class DtoConverter
{
    public static ProbationProject MapProjectResponseToModel(ProjectResponse response)
    {
        return new ProbationProject
        {
            Id = response.Id,
            Title = response.Title,
            StartDate = response.StartDate.ToDateTime(new TimeOnly()),
            Description = response.Description,
            Interns = response.Interns.Select(MapInternResponseToModel).ToList()
        };
    }
    
    public static ProbationCourse MapCourseResponseToModel(CourseResponse response)
    {
        return new ProbationCourse
        {
            Id = response.Id,
            Title = response.Title,
            Description = response.Description,
            Interns = response.Interns.Select(MapInternResponseToModel).ToList()
        };
    }
    
    public static Intern MapInternResponseToModel(InternResponse response)
    {
        var model = new Intern
        {
            Id = response.Id,
            FirstName = response.FirstName,
            LastName = response.LastName,
            IsMale = response.IsMale,
            Email = response.Email,
            Phone = response.Phone,
            BirthDate = response.BirthDate.ToDateTime(new TimeOnly())
        };
        if (response.ProbationProjectId.HasValue)
        {
            model.ProbationProject = new BriefProjectInfo()
            {
                Id = response.ProbationProjectId.Value,
                Title = response.ProbationProjectTitle!
            };
        }
        if (response.ProbationCourseId.HasValue)
        {
            model.ProbationCourse = new BriefCourseInfo()
            {
                Id = response.ProbationCourseId.Value,
                Title = response.ProbationCourseTitle!
            };
        }

        return model;
    }
}