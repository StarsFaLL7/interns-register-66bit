using Application.Interfaces;
using Application.Services;
using Domain.DataQuery;
using Domain.Entities;
using InternRegister.Controllers.Base;
using InternRegister.Controllers.Base.Responses;
using InternRegister.Controllers.ProbationCourses.Requests;
using InternRegister.Controllers.ProbationCourses.Responses;
using InternRegister.Controllers.Utility;
using Microsoft.AspNetCore.Mvc;

namespace InternRegister.Controllers.ProbationCourses;

[Route("api/courses")]
public class ProbationCoursesController : Controller
{
    private readonly BaseService<ProbationCourse> _coursesItemService;
    private readonly ICourseService _courseService;

    public ProbationCoursesController(BaseService<ProbationCourse> coursesItemService,
        ICourseService courseService)
    {
        _coursesItemService = coursesItemService;
        _courseService = courseService;
    }
    
    /// <summary>
    /// Получить отфильтрованный список направлений
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(CoursesListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCoursesFiltered([FromQuery] int? skip, [FromQuery] int? take,
        [FromQuery] string? orderBy, [FromQuery] bool? ascending, [FromQuery] string? search)
    {
        var dataQueryParams = DtoConverter.ConvertBasicDataQuery<ProbationCourse>(skip, take, orderBy, ascending ?? true);
        dataQueryParams.Filters = [];
        if (!string.IsNullOrWhiteSpace(search))
        {
            dataQueryParams.Filters.Add(i => i.Title.ToLower().Contains(search.ToLower()));
        }
        dataQueryParams.IncludeParams = new IncludeParams<ProbationCourse>
        {
            IncludeProperties = [p => p.Interns]
        };
        if (orderBy == "InternsCount")
        {
            dataQueryParams.Sorting = new SortingParams<ProbationCourse>
            {
                OrderBy = c => c.Interns!.Count,
                Ascending = ascending ?? false
            };
        }
        else if (string.IsNullOrWhiteSpace(orderBy))
        {
            dataQueryParams.Sorting = new SortingParams<ProbationCourse>
            {
                OrderBy = c => c.Title,
                Ascending = ascending ?? false
            };
        }
        
        var found = await _coursesItemService.GetAsync(dataQueryParams);
        var foundTotal = await _coursesItemService.GetCountAsync(dataQueryParams);
        
        return Ok(new CoursesListResponse
        {
            Courses = found.Select(DtoConverter.MapCourseToResponse)
                .ToArray(),
            WithoutPagingCount = foundTotal
        });
    }
    
    /// <summary>
    /// Получить направление по его уникальному идентификатору
    /// </summary>
    [HttpGet("{courseId:guid}")]
    [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCourseById([FromRoute] Guid courseId)
    {
        var found = await GetFullCourseInfoAsync(courseId);
        if (found == null)
        {
            return SharedResponses.NotFoundObjectResponse<CourseResponse>(courseId);
        }
        return Ok(DtoConverter.MapCourseToResponse(found));
    }
    
    /// <summary>
    /// Создать новое направление
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewCourse([FromBody] CreateUpdateCourseRequest dto,
        [FromServices] BaseService<Intern> internService)
    {
        var course = new ProbationCourse
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Interns = []
        };
        await _coursesItemService.SaveAsync(course);
        await _courseService.ReapplyInternsToCourseAsync(course, dto.InternIds);
        return Ok(DtoConverter.MapCourseToResponse(course));
    }
    
    /// <summary>
    /// Обновить информацию о существующем направлении
    /// </summary>
    [HttpPut("{courseId:guid}")]
    [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCourse([FromBody] CreateUpdateCourseRequest dto, [FromRoute] Guid courseId)
    {
        var foundCourse = await GetFullCourseInfoAsync(courseId);
        if (foundCourse == null)
        {
            return SharedResponses.NotFoundObjectResponse<ProbationCourse>(courseId);
        }
        foundCourse.Title = dto.Title;
        foundCourse.Description = dto.Description;
        var interns = foundCourse.Interns;
        foundCourse.Interns = [];
        await _coursesItemService.SaveAsync(foundCourse);
        foundCourse.Interns = interns;
        await _courseService.ReapplyInternsToCourseAsync(foundCourse, dto.InternIds);
        
        return Ok(DtoConverter.MapCourseToResponse(foundCourse));
    }
    
    /// <summary>
    /// Удалить направление, если у него нет связанных стажеров
    /// </summary>
    [HttpDelete("{courseId:guid}")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCourse([FromRoute] Guid courseId)
    {
        var foundCourse = await GetFullCourseInfoAsync(courseId);
        if (foundCourse == null)
        {
            return SharedResponses.NotFoundObjectResponse<ProbationProject>(courseId);
        }

        if (foundCourse.Interns!.Count > 0)
        {
            return BadRequest(new BaseStatusResponse
            {
                Completed = false,
                Message = $"Course \"{foundCourse.Title}\" has interns connected, so can't be deleted."
            });
        }
        
        await _coursesItemService.TryRemoveAsync(foundCourse.Id);
        
        return Ok(new BaseStatusResponse
        {
            Completed = true,
            Message = $"Course \"{foundCourse.Title}\" was successfully deleted."
        });
    }

    private async Task<ProbationCourse?> GetFullCourseInfoAsync(Guid courseId)
    {
        return await _coursesItemService.GetByIdOrDefaultAsync(courseId, new IncludeParams<ProbationCourse>
        {
            IncludeProperties = [i => i.Interns]
        });
    }
}