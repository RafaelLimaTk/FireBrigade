using CommunityToolkit.Maui;
using FireBrigade.Data;
using FireBrigade.Domain.Interfaces;
using FireBrigade.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FireBrigade
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.Services.AddSingleton<IUserBrigadeRepository, UserBrigadeRepository>();
            builder.Services.AddSingleton<IEmergencyBrigadeRepository, EmergencyBrigadeRepository>();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddDbContext<FireBrigadeContext>(options =>
                        options.UseSqlite("Data Source=fireBrigade.db"));
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
