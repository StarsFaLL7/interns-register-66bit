using Application.Services;
using Domain.DataQuery;
using Domain.Entities;
using InternRegister.Controllers.Base;
using InternRegister.Controllers.Base.Responses;
using InternRegister.Controllers.Interns.Requests;
using InternRegister.Controllers.Interns.Responses;
using InternRegister.Controllers.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternRegister.Controllers.Interns;

[Route("api/interns")]
public class InternsController : Controller
{
    private readonly BaseService<Intern> _internService;

    public InternsController(BaseService<Intern> internService)
    {
        _internService = internService;
    }
    
    /// <summary>
    /// Получить отфильтрованный список стажеров
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(InternListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInternsFiltered([FromQuery] int? skip, [FromQuery] int? take,
        [FromQuery] string? orderBy, [FromQuery] bool? ascending, [FromQuery] string? search,
        [FromQuery] Guid? courseId, [FromQuery] Guid? projectId)
    {
        var dataQueryParams = DtoConverter.ConvertBasicDataQuery<Intern>(skip, take, orderBy, ascending ?? true);
        dataQueryParams.Filters = [];
        dataQueryParams.IncludeParams = new IncludeParams<Intern>
        {
            IncludeProperties = [p => p.ProbationCourse, p => p.ProbationProject]
        };
        if (!string.IsNullOrWhiteSpace(search))
        {
            dataQueryParams.Filters.Add(i => (i.LastName + "" + i.FirstName).ToLower().Contains(search.ToLower()));
        }
        if (courseId != null)
        {
            dataQueryParams.Filters.Add(i => i.ProbationCourseId == courseId);
        }
        if (projectId != null)
        {
            dataQueryParams.Filters.Add(i => i.ProbationProjectId == projectId);
        }

        var found = await _internService.GetAsync(dataQueryParams);
        var foundTotal = await _internService.GetCountAsync(dataQueryParams);

        return Ok(new InternListResponse
        {
            Interns = found.Select(DtoConverter.MapInternToResponse)
                .ToArray(),
            WithoutPagingCount = foundTotal
        });
    }
    
    /// <summary>
    /// Получить стажера по его уникальному идентификатору
    /// </summary>
    [HttpGet("{internId:guid}")]
    [ProducesResponseType(typeof(InternResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetInternById([FromRoute] Guid internId)
    {
        var found = await GetFullInternInfoAsync(internId);
        if (found == null)
        {
            return SharedResponses.NotFoundObjectResponse<Intern>(internId);
        }
        return Ok(DtoConverter.MapInternToResponse(found));
    }
    
    /// <summary>
    /// Создать нового стажера
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(InternResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewIntern([FromBody] CreateUpdateInternRequest dto)
    {
        var dt = DateTimeOffset.FromUnixTimeSeconds(dto.BirthDateTimeStamp).Date;
        var intern = new Intern
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            IsMale = dto.IsMale,
            Email = dto.Email,
            Phone = dto.Phone,
            BirthDate = new DateOnly(dt.Year, dt.Month, dt.Day),
            ProbationCourseId = dto.ProbationCourseId,
            ProbationProjectId = dto.ProbationProjectId
        };
        
        await _internService.SaveAsync(intern);
        var internSaved = await GetFullInternInfoAsync(intern.Id);
        
        return Ok(DtoConverter.MapInternToResponse(internSaved!));
    }
    
    /// <summary>
    /// Обновить информацию о существующем стажере
    /// </summary>
    [HttpPut("{internId:guid}")]
    [ProducesResponseType(typeof(InternResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateIntern([FromBody] CreateUpdateInternRequest dto, [FromRoute] Guid internId)
    {
        var foundIntern = await GetFullInternInfoAsync(internId);
        if (foundIntern == null)
        {
            return SharedResponses.NotFoundObjectResponse<Intern>(internId);
        }
        foundIntern.ProbationCourse = null!;
        foundIntern.ProbationProject = null!;
        
        var uniqueCheck = await _internService.GetAsync(new DataQueryParams<Intern>
        {
            Expression = i => i.Email == dto.Email || (!string.IsNullOrWhiteSpace(dto.Phone) && i.Phone == dto.Phone),
            Filters = [i => i.Id != internId]
        });
        if (uniqueCheck.Length > 0)
        {
            var msg = $"Уже существует стажер с указанным номером телефона \"{dto.Phone}\" - {uniqueCheck[0].LastName} {uniqueCheck[0].FirstName}";
            var sameEmail = uniqueCheck.FirstOrDefault(i => i.Email == dto.Email);
            if (sameEmail != null)
            {
                msg = $"Уже существует стажер с указанным email адресом \"{dto.Email}\" - {sameEmail.LastName} {sameEmail.FirstName}";
            }
            return BadRequest(new BaseStatusResponse
            {
                Completed = false,
                Message = msg
            });
        }
        
        DtoConverter.CopyInternProperties(dto, foundIntern);
        await _internService.SaveAsync(foundIntern);
        
        return Ok(DtoConverter.MapInternToResponse(foundIntern));
    }
    
    /// <summary>
    /// Изменить проект стажера с идентификатором internId
    /// </summary>
    [HttpPut("{internId:guid}/project")]
    [ProducesResponseType(typeof(InternResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApplyInternToProject([FromRoute] Guid internId, [FromBody] UpdateInternProjectRequest dto,
        [FromServices] BaseService<ProbationProject> projectService)
    {
        var foundIntern = await GetFullInternInfoAsync(internId);
        if (foundIntern == null)
        {
            return SharedResponses.NotFoundObjectResponse<Intern>(internId);
        }

        if (!dto.ProjectId.HasValue)
        {
            foundIntern.ProbationProjectId = null;
        }
        else
        {
            var foundProject = await projectService.GetByIdOrDefaultAsync(dto.ProjectId.Value);
            if (foundProject == null)
            {
                return SharedResponses.NotFoundObjectResponse<ProbationProject>(dto.ProjectId.Value);
            }
            foundIntern.ProbationProjectId = null;
            foundIntern.ProbationProject = foundProject;
        }
        
        await _internService.SaveAsync(foundIntern);
        
        return Ok(DtoConverter.MapInternToResponse(foundIntern));
    }
    
    /// <summary>
    /// Изменить направление стажера с идентификатором internId
    /// </summary>
    [HttpPut("{internId:guid}/course")]
    [ProducesResponseType(typeof(InternResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApplyInternToCourse([FromRoute] Guid internId, [FromBody] UpdateInternCourseRequest dto, 
        [FromServices] BaseService<ProbationCourse> courseService)
    {
        var foundIntern = await GetFullInternInfoAsync(internId);
        if (foundIntern == null)
        {
            return SharedResponses.NotFoundObjectResponse<Intern>(internId);
        }

        if (!dto.CourseId.HasValue)
        {
            foundIntern.ProbationCourseId = null;
        }
        else
        {
            var foundCourse = await courseService.GetByIdOrDefaultAsync(dto.CourseId.Value);
            if (foundCourse == null)
            {
                return SharedResponses.NotFoundObjectResponse<ProbationCourse>(dto.CourseId.Value);
            }
            foundIntern.ProbationCourseId = null;
            foundIntern.ProbationCourse = foundCourse;
        }
        
        await _internService.SaveAsync(foundIntern);
        
        return Ok(DtoConverter.MapInternToResponse(foundIntern));
    }
    
    /// <summary>
    /// Удалить стажера
    /// </summary>
    [HttpDelete("{internId:guid}")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteIntern([FromRoute] Guid internId)
    {
        var foundIntern = await _internService.GetByIdOrDefaultAsync(internId);
        if (foundIntern == null)
        {
            return SharedResponses.NotFoundObjectResponse<Intern>(internId);
        }
        
        await _internService.TryRemoveAsync(foundIntern.Id);
        
        return Ok(new BaseStatusResponse
        {
            Completed = true,
            Message = $"Intern \"{foundIntern.LastName} {foundIntern.FirstName}\" was successfully deleted."
        });
    }

    private async Task<Intern?> GetFullInternInfoAsync(Guid internId)
    {
        return await _internService.GetByIdOrDefaultAsync(internId, new IncludeParams<Intern>
        {
            IncludeProperties = [i => i.ProbationCourse, i => i.ProbationProject]
        });
    }
}