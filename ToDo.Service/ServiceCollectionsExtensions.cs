using ToDo.Services.DataBase;
using ToDo.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Web
{
    public static class ServiceCollectionsExtensions
    {    
        public static IServiceCollection AddToDoService(this IServiceCollection services, IConfiguration configuration)
        {
            var pathToDb = configuration.GetConnectionString("local");

            return services.AddScoped<IDbService, DbService>()
                 .AddDbContext<DbContext, ToDoDbContext>(dbOptions =>
                 {
                     dbOptions.UseSqlite($"Data Source={pathToDb}");
                 });
        }
    }
}
