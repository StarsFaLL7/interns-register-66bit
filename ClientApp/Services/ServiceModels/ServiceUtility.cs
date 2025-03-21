using ClientApp.Services.Http.Models;

namespace ClientApp.Services.ServiceModels;

public static class ServiceUtility
{
    public static HttpRequestData CreateRequestFromFiltersQuery(FilteredListRequest query, HttpMethod method, string apiUrl)
    {
        var request = new HttpRequestData
        {
            Method = method,
            EndpointPath = apiUrl,
            QueryParameterList =
            {
                new KeyValuePair<string, string>("skip", query.Skip.ToString()),
                new KeyValuePair<string, string>("take", query.Take.ToString()),
                new KeyValuePair<string, string>("orderBy", query.OrderBy ?? ""),
                new KeyValuePair<string, string>("ascending", query.Ascending.ToString()),
                new KeyValuePair<string, string>("search", query.Search ?? "")
            }
        };
        if (query.AdditionalQueryParams.Count > 0)
        {
            foreach (var kvPair in query.AdditionalQueryParams)
            {
                request.QueryParameterList.Add(kvPair);
            }
        }

        return request;
    }
}