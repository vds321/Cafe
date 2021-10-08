using Cafe.ViewModels;
using Cafe.ViewModels.MainViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Cafe.Service.Registrator
{
    internal static class ViewModelRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<MainWindowViewModel>()
        ;

    }
}
