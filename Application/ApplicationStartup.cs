using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Application;

public static class ApplicationStartup
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.TryAddScoped<BaseService<Intern>>();
        services.TryAddScoped<BaseService<ProbationProject>>();
        services.TryAddScoped<BaseService<ProbationCourse>>();
        services.TryAddScoped<ICourseService, CourseService>();
        services.TryAddScoped<IProjectService, ProjectsService>();
        return services;
    }
}