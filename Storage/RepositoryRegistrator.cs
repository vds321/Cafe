using Microsoft.Extensions.DependencyInjection;
using Storage.Entities;
using Storage.Interface;

namespace Storage
{
    public static class RepositoryRegistrator
    {
        public static IServiceCollection AddRepositoryInDB(this IServiceCollection services) => services
            .AddTransient<IRepository<Dish>, DishesRepository>()
            .AddTransient<IRepository<Order>, OrdersRepository>()
            .AddTransient<IRepository<Composition>, CompositionsRepository>()
            .AddTransient<IRepository<Category>, CategoryRepository>()
            .AddTransient<IRepository<Product>, ProductRepository>()
            ;

    }
}
