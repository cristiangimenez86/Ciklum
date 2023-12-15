using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sportradar.Services.DbContexts;
using Sportradar.Services.Repositories;
using Sportradar.Services.Validators;

namespace Sportradar.Services.DI
{
    public static class ServicesConfiguration
    {
        public static void AddScoreBoardService(this IServiceCollection services)
        {
            services.AddDbContext<ScoreBoardDbContext>(options =>
            {
                options.UseInMemoryDatabase("ScoreBoard");
            });

            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IScoreBoardService, ScoreBoardService>();
            services.AddScoped<IValidator, Validator>();
        }
    }
}
