using Cafe.ViewModels;
using Cafe.ViewModels.MainViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Cafe.Service.Registrator
{
    internal class ViewModelLocator
    {
        public static MainWindowViewModel MainWindowViewModel => App.Services.GetRequiredService<MainWindowViewModel>();

    }
}
