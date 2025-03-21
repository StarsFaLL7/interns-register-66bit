using Domain.Entities;

namespace Application.Interfaces;

public interface ICourseService
{
    public Task ReapplyInternsToCourseAsync(ProbationCourse course, Guid[] internIds);
}