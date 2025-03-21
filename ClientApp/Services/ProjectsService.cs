using ClientApp.Models;
using ClientApp.Services.Http;
using ClientApp.Services.Http.Models;
using ClientApp.Services.Interfaces;
using ClientApp.Services.ServiceModels;
using InternRegister.Controllers.Base.Responses;
using InternRegister.Controllers.ProbationCourses.Responses;
using InternRegister.Controllers.ProbationProjects.Requests;
using InternRegister.Controllers.ProbationProjects.Responses;

namespace ClientApp.Services;

public class ProjectsService : IItemService<ProbationProject>
{
    private readonly IHttpService _httpService;

    public ProjectsService(IHttpService httpService)
    {
        _httpService = httpService;
    }
    
    public async Task<ServiceActionResult<List<ProbationProject>>> GetAllAsync()
    {
        var response = await _httpService.SendHttpRequestAsync<ProjectsListResponse>(new HttpRequestData
        {
            Method = HttpMethod.Get,
            EndpointPath = "api/projects"
        });
        
        var result = new ServiceActionResult<List<ProbationProject>>
        {
            Item = null,
            Completed = response.Completed,
            Message = response.Message
        };
        if (response.Item != null)
        {
            result.Item = response.Item.Projects.Select(DtoConverter.MapProjectResponseToModel).ToList();
        }
        return result;
    }
    
    public async Task<ServiceActionResult<FilteredList<ProbationProject>>> GetFilteredAsync(FilteredListRequest query)
    {
        var request = ServiceUtility.CreateRequestFromFiltersQuery(query, HttpMethod.Get, "api/projects");
        var response = await _httpService.SendHttpRequestAsync<ProjectsListResponse>(request);
        
        var result = new ServiceActionResult<FilteredList<ProbationProject>>
        {
            Item = null,
            Completed = response.Completed,
            Message = response.Message
        };
        if (response.Item != null)
        {
            result.Item = new FilteredList<ProbationProject>
            {
                Items = response.Item.Projects.Select(DtoConverter.MapProjectResponseToModel).ToList(),
                WithoutPagingCount = response.Item.WithoutPagingCount
            };
        }
        return result;
    }

    public async Task<ServiceActionResult<ProbationProject>> GetAggregatedByIdAsync(Guid id)
    {
        var response = await _httpService.SendHttpRequestAsync<ProjectResponse>(new HttpRequestData
        {
            Method = HttpMethod.Get,
            EndpointPath = $"api/projects/{id}"
        });
        return ProcessProjectResponse(response);
    }

    public async Task<ServiceActionResult<ProbationProject>> SaveAsync(ProbationProject item)
    {
        var response = await _httpService.SendHttpRequestAsync<ProjectResponse>(new HttpRequestData
        {
            Method = HttpMethod.Put,
            EndpointPath = $"api/projects/{item.Id}", 
            Body = new CreateUpdateProjectRequest
            {
                Title = item.Title,
                Description = item.Description,
                StartDateTimestamp = ((DateTimeOffset)item.StartDate.ToUniversalTime()).ToUnixTimeSeconds(),
                InternIds = item.Interns?.Select(i => i.Id).ToArray() ?? []
            }
        });
        return ProcessProjectResponse(response);
    }

    public async Task<ServiceActionResult<ProbationProject>> CreateAsync(ProbationProject item)
    {
        var response = await _httpService.SendHttpRequestAsync<ProjectResponse>(new HttpRequestData
        {
            Method = HttpMethod.Post,
            EndpointPath = "api/projects", 
            Body = new CreateUpdateProjectRequest
            {
                Title = item.Title,
                Description = item.Description,
                StartDateTimestamp = ((DateTimeOffset)item.StartDate.ToUniversalTime()).ToUnixTimeSeconds(),
                InternIds = item.Interns?.Select(i => i.Id).ToArray() ?? []
            }
        });
        return ProcessProjectResponse(response);
    }

    public async Task<ServiceActionResult<ProbationProject>> DeleteAsync(Guid id)
    {
        var response = await _httpService.SendHttpRequestAsync<BaseStatusResponse>(new HttpRequestData
        {
            Method = HttpMethod.Delete,
            EndpointPath = $"api/projects/{id}"
        });
        var result = new ServiceActionResult<ProbationProject>
        {
            Item = null,
            Completed = response.Completed,
            Message = response.Message
        };
        return result;
    }

    private ServiceActionResult<ProbationProject> ProcessProjectResponse(ServiceActionResult<ProjectResponse> response)
    {
        var result = new ServiceActionResult<ProbationProject>
        {
            Item = null,
            Completed = response.Completed,
            Message = response.Message
        };
        if (response.Item != null)
        {
            result.Item = DtoConverter.MapProjectResponseToModel(response.Item);
        }
        return result;
    }
}