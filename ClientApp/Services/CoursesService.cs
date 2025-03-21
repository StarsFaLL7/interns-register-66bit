using ClientApp.Models;
using ClientApp.Services.Http;
using ClientApp.Services.Http.Models;
using ClientApp.Services.Interfaces;
using ClientApp.Services.ServiceModels;
using InternRegister.Controllers.Base.Responses;
using InternRegister.Controllers.ProbationCourses.Requests;
using InternRegister.Controllers.ProbationCourses.Responses;

namespace ClientApp.Services;

public class CoursesService : IItemService<ProbationCourse>
{
    private readonly IHttpService _httpService;

    public CoursesService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<ServiceActionResult<List<ProbationCourse>>> GetAllAsync()
    {
        var response = await _httpService.SendHttpRequestAsync<CoursesListResponse>(new HttpRequestData
        {
            Method = HttpMethod.Get,
            EndpointPath = "api/courses"
        });
        var result = new ServiceActionResult<List<ProbationCourse>>
        {
            Item = null,
            Completed = response.Completed,
            Message = response.Message
        };
        if (response.Item != null)
        {
            result.Item = response.Item.Courses.Select(DtoConverter.MapCourseResponseToModel).ToList();
        }
        return result;
    }

    public async Task<ServiceActionResult<FilteredList<ProbationCourse>>> GetFilteredAsync(FilteredListRequest query)
    {
        var request = ServiceUtility.CreateRequestFromFiltersQuery(query, HttpMethod.Get, "api/courses");
        var response = await _httpService.SendHttpRequestAsync<CoursesListResponse>(request);
        
        var result = new ServiceActionResult<FilteredList<ProbationCourse>>
        {
            Item = null,
            Completed = response.Completed,
            Message = response.Message
        };
        if (response.Item != null)
        {
            result.Item = new FilteredList<ProbationCourse>
            {
                Items = response.Item.Courses.Select(DtoConverter.MapCourseResponseToModel).ToList(),
                WithoutPagingCount = response.Item.WithoutPagingCount
            };
        }
        return result;
    }

    public async Task<ServiceActionResult<ProbationCourse>> GetAggregatedByIdAsync(Guid id)
    {
        var response = await _httpService.SendHttpRequestAsync<CourseResponse>(new HttpRequestData
        {
            Method = HttpMethod.Get,
            EndpointPath = $"api/courses/{id}"
        });
        return ProcessCourseResponse(response);
    }

    public async Task<ServiceActionResult<ProbationCourse>> SaveAsync(ProbationCourse item)
    {
        var response = await _httpService.SendHttpRequestAsync<CourseResponse>(new HttpRequestData
        {
            Method = HttpMethod.Put,
            EndpointPath = $"api/courses/{item.Id}", 
            Body = new CreateUpdateCourseRequest
            {
                Title = item.Title,
                Description = item.Description,
                InternIds = item.Interns?.Select(i => i.Id).ToArray() ?? []
            }
        });
        return ProcessCourseResponse(response);
    }

    public async Task<ServiceActionResult<ProbationCourse>> CreateAsync(ProbationCourse item)
    {
        var response = await _httpService.SendHttpRequestAsync<CourseResponse>(new HttpRequestData
        {
            Method = HttpMethod.Post,
            EndpointPath = "api/courses", 
            Body = new CreateUpdateCourseRequest
            {
                Title = item.Title,
                Description = item.Description,
                InternIds = item.Interns?.Select(i => i.Id).ToArray() ?? []
            }
        });
        return ProcessCourseResponse(response);
    }

    public async Task<ServiceActionResult<ProbationCourse>> DeleteAsync(Guid id)
    {
        var response = await _httpService.SendHttpRequestAsync<BaseStatusResponse>(new HttpRequestData
        {
            Method = HttpMethod.Delete,
            EndpointPath = $"api/courses/{id}"
        });
        var result = new ServiceActionResult<ProbationCourse>
        {
            Item = null,
            Completed = response.Completed,
            Message = response.Message
        };
        return result;
    }
    
    private ServiceActionResult<ProbationCourse> ProcessCourseResponse(ServiceActionResult<CourseResponse> response)
    {
        var result = new ServiceActionResult<ProbationCourse>
        {
            Item = null,
            Completed = response.Completed,
            Message = response.Message
        };
        if (response.Item != null)
        {
            result.Item = DtoConverter.MapCourseResponseToModel(response.Item);
        }
        return result;
    }
}