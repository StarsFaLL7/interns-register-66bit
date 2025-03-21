using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Services.Http;
using ClientApp.Services.Interfaces;

namespace ClientApp;

public static class StartupProperties
{
    public static IServiceCollection AddHttpService(this IServiceCollection services)
    {
        services.AddScoped<IHttpService, HttpService>();

        return services;
    }
    
    public static IServiceCollection AddItemServices(this IServiceCollection services)
    {
        services.AddScoped<IItemService<Intern>, InternsService>();
        services.AddScoped<IItemService<ProbationCourse>, CoursesService>();
        services.AddScoped<IItemService<ProbationProject>, ProjectsService>();

        return services;
    }
}