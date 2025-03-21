using ClientApp.Models;
using ClientApp.Services.Http;
using ClientApp.Services.Http.Models;
using ClientApp.Services.Interfaces;
using ClientApp.Services.ServiceModels;
using InternRegister.Controllers.Base.Responses;
using InternRegister.Controllers.Interns.Requests;
using InternRegister.Controllers.Interns.Responses;
using InternRegister.Controllers.ProbationCourses.Requests;
using InternRegister.Controllers.ProbationCourses.Responses;

namespace ClientApp.Services;

public class InternsService : IItemService<Intern>
{
    private readonly IHttpService _httpService;

    public InternsService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<ServiceActionResult<List<Intern>>> GetAllAsync()
    {
        var response = await _httpService.SendHttpRequestAsync<InternListResponse>(new HttpRequestData
        {
            Method = HttpMethod.Get,
            EndpointPath = "api/interns"
        });
        
        var result = new ServiceActionResult<List<Intern>>
        {
            Item = null,
            Completed = response.Completed,
            Message = response.Message
        };
        if (response.Item != null)
        {
            result.Item = response.Item.Interns.Select(DtoConverter.MapInternResponseToModel).ToList();
        }
        return result;
    }

    public async Task<ServiceActionResult<FilteredList<Intern>>> GetFilteredAsync(FilteredListRequest query)
    {
        var request = ServiceUtility.CreateRequestFromFiltersQuery(query, HttpMethod.Get, "api/interns");
        var response = await _httpService.SendHttpRequestAsync<InternListResponse>(request);
        
        var result = new ServiceActionResult<FilteredList<Intern>>
        {
            Item = null,
            Completed = response.Completed,
            Message = response.Message
        };
        if (response.Item != null)
        {
            result.Item = new FilteredList<Intern>
            {
                Items = response.Item.Interns.Select(DtoConverter.MapInternResponseToModel).ToList(),
                WithoutPagingCount = response.Item.WithoutPagingCount
            };
        }
        return result;
    }

    public async Task<ServiceActionResult<Intern>> GetAggregatedByIdAsync(Guid id)
    {
        var response = await _httpService.SendHttpRequestAsync<InternResponse>(new HttpRequestData
        {
            Method = HttpMethod.Get,
            EndpointPath = $"api/interns/{id}"
        });
        return ProcessInternResponse(response);
    }

    public async Task<ServiceActionResult<Intern>> SaveAsync(Intern item)
    {
        var response = await _httpService.SendHttpRequestAsync<InternResponse>(new HttpRequestData
        {
            Method = HttpMethod.Put,
            EndpointPath = $"api/interns/{item.Id}", 
            Body = new CreateUpdateInternRequest
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                IsMale = item.IsMale,
                Email = item.Email,
                Phone = item.Phone,
                BirthDateTimeStamp = ((DateTimeOffset)item.BirthDate.ToUniversalTime()).ToUnixTimeSeconds(),
                ProbationCourseId = item.ProbationCourse?.Id,
                ProbationProjectId = item.ProbationProject?.Id
            }
        });
        return ProcessInternResponse(response);
    }

    public async Task<ServiceActionResult<Intern>> CreateAsync(Intern item)
    {
        var response = await _httpService.SendHttpRequestAsync<InternResponse>(new HttpRequestData
        {
            Method = HttpMethod.Post,
            EndpointPath = "api/interns", 
            Body = new CreateUpdateInternRequest
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                IsMale = item.IsMale,
                Email = item.Email,
                Phone = item.Phone,
                BirthDateTimeStamp = ((DateTimeOffset)item.BirthDate.ToUniversalTime()).ToUnixTimeSeconds(),
                ProbationCourseId = item.ProbationCourse?.Id,
                ProbationProjectId = item.ProbationProject?.Id
            }
        });
        return ProcessInternResponse(response);
    }

    public async Task<ServiceActionResult<Intern>> DeleteAsync(Guid id)
    {
        var response = await _httpService.SendHttpRequestAsync<BaseStatusResponse>(new HttpRequestData
        {
            Method = HttpMethod.Delete,
            EndpointPath = $"api/interns/{id}"
        });
        var result = new ServiceActionResult<Intern>
        {
            Item = null,
            Completed = response.Completed,
            Message = response.Message
        };
        return result;
    }
    
    private ServiceActionResult<Intern> ProcessInternResponse(ServiceActionResult<InternResponse> response)
    {
        var result = new ServiceActionResult<Intern>
        {
            Item = null,
            Completed = response.Completed,
            Message = response.Message
        };
        if (response.Item != null)
        {
            result.Item = DtoConverter.MapInternResponseToModel(response.Item);
        }
        return result;
    }
}