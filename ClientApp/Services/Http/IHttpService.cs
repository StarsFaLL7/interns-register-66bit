using ClientApp.Services.Http.Models;
using ClientApp.Services.ServiceModels;

namespace ClientApp.Services.Http;

/// <summary>
/// Интерфейс для сервиса, реализующего отправку запросов на api сервер
/// </summary>
public interface IHttpService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<ServiceActionResult<T>> SendHttpRequestAsync<T>(HttpRequestData requestData) where T : class;
}