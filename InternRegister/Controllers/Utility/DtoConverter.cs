using Domain.DataQuery;
using Domain.Entities;
using Domain.Interfaces;
using InternRegister.Controllers.Interns.Requests;
using InternRegister.Controllers.Interns.Responses;
using InternRegister.Controllers.ProbationCourses.Responses;
using InternRegister.Controllers.ProbationProjects.Responses;

namespace InternRegister.Controllers.Utility;

public static class DtoConverter
{
    public static DataQueryParams<T> ConvertBasicDataQuery<T>(int? skip, int? take, string? orderProperty, bool ascending) where T : class, IHasId
    {
        var queryParams = new DataQueryParams<T>
        {
            Paging = null,
            Sorting = null,
            Filters = null,
            IncludeParams = null
        };
        if (skip != null && take != null)
        {
            queryParams.Paging = new PagingParams
            {
                Skip = skip ?? 0,
                Take = take ?? 10
            };
        }

        if (!string.IsNullOrWhiteSpace(orderProperty))
        {
            queryParams.Sorting = new SortingParams<T>
            {
                PropertyName = orderProperty,
                Ascending = ascending
            };
        }
        
        return queryParams;
    }
    
    public static InternResponse MapInternToResponse(Intern intern)
    {
        return new InternResponse
        {
            Id = intern.Id,
            FirstName = intern.FirstName,
            LastName = intern.LastName,
            IsMale = intern.IsMale,
            Email = intern.Email,
            Phone = intern.Phone,
            BirthDate = intern.BirthDate,
            ProbationCourseId = intern.ProbationCourseId,
            ProbationCourseTitle = intern.ProbationCourse?.Title,
            ProbationProjectId = intern.ProbationProjectId,
            ProbationProjectTitle = intern.ProbationProject?.Title
        };
    }
    
    public static ProjectResponse MapProjectToResponse(ProbationProject project)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            Interns = project.Interns == null ? [] : project.Interns.Select(MapInternToResponse).ToArray(),
            StartDate = project.StartDate
        };
    }
    
    public static CourseResponse MapCourseToResponse(ProbationCourse course)
    {
        return new CourseResponse
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Interns = course.Interns == null ? [] : course.Interns.Select(MapInternToResponse).ToArray()
        };
    }
    
    public static void CopyInternProperties(CreateUpdateInternRequest from, Intern to)
    {
        var dt = DateTimeOffset.FromUnixTimeSeconds(from.BirthDateTimeStamp).Date;
        to.FirstName = from.FirstName;
        to.LastName = from.LastName;
        to.IsMale = from.IsMale;
        to.Email = from.Email;
        to.Phone = from.Phone;
        to.BirthDate = new DateOnly(dt.Year, dt.Month, dt.Day);
        to.ProbationCourseId = from.ProbationCourseId;
        to.ProbationProjectId = from.ProbationProjectId;
    }
}