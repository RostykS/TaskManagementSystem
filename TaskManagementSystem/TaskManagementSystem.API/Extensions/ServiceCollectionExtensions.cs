using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.API.Data;
using TaskManagementSystem.API.Profiles;
using TaskManagementSystem.API.Repositories;
using TaskManagementSystem.API.Services;

namespace TaskManagementSystem.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<TaskDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // AutoMapper
            services.AddAutoMapper(typeof(TaskMappingProfile));

            // Repositories
            services.AddScoped<ITaskRepository, TaskRepository>();

            // Services
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IServiceBusHandler, ServiceBusHandler>();

            return services;
        }
    }
}