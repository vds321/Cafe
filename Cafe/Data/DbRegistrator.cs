using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Storage;
using Storage.Context;
using System;

namespace Cafe.Data
{
    public static class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration Configuration) => services
            .AddDbContext<CafeDB>(options =>
            {
                var type = Configuration["Type"];
                switch (type)
                {
                    case null: throw new InvalidOperationException("Не определен тип БД");
                    default: throw new InvalidOperationException($"Тип подключения {type} не поддерживается");
                    case "MSSQL":
                        options.UseSqlServer(Configuration.GetConnectionString(type));
                        break;
                }

            })
            .AddTransient<DbInitializer>()
            .AddRepositoryInDB()
            ;
    }
}
