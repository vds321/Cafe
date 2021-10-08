using Cafe.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cafe.Service.Registrator
{
    internal static class ServiceRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddTransient<IUserDialog, UserDialogService>();
    }
}
