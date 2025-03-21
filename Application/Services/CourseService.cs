using Application.Interfaces;
using Domain.DataQuery;
using Domain.Entities;

namespace Application.Services;

public class CourseService : ICourseService
{
    private readonly BaseService<Intern> _internService;

    public CourseService(BaseService<Intern> internService)
    {
        _internService = internService;
    }
    
    public async Task ReapplyInternsToCourseAsync(ProbationCourse course, Guid[] internIds)
    {
        course.Interns ??= [];
        var newInterns = await _internService.GetAsync(new DataQueryParams<Intern>
        {
            Expression = i => internIds.Contains(i.Id)
        });
        foreach (var intern in course.Interns.UnionBy(newInterns, i => i.Id))
        {
            intern.ProbationCourse = null;
            intern.ProbationCourseId = null;
            if (internIds.Contains(intern.Id))
            {
                intern.ProbationCourseId = course.Id;
            }

            await _internService.SaveAsync(intern);
        }
    }
}