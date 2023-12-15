using Microsoft.Extensions.DependencyInjection;
using Sportradar.Services.DbContexts;
using Sportradar.Services.Repositories;

namespace Sportradar.Services.DI
{
    public static class ServicesConfiguration
    {
        public static void AddScoreBoardService(this IServiceCollection services)
        {
            services.AddDbContext<ScoreBoardDbContext>();

            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IScoreBoardService, ScoreBoardService>();
        }
    }
}
