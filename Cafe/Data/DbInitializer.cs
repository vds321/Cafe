using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Storage.Context;
using Storage.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cafe.Data
{
    class DbInitializer
    {
        private readonly string[] _CategoriesList = DbStartDataInitialize.CategoriesList;
        private readonly Dictionary<string, double> _ProductsList = DbStartDataInitialize.ProductsList;
        private readonly Dictionary<string, (double weigth, decimal price, string category, List<string> components)> _DishesList = DbStartDataInitialize.DishesList;
        private readonly Dictionary<int, (string data, Dictionary<string, int> dishes_qty)> _OrdersList = DbStartDataInitialize.OrdersList;


        private readonly CafeDB _db;
        private readonly ILogger<DbInitializer> _Logger;

        private Product[] _Products;
        private Category[] _Categories;
        private Dish[] _Dishes;
        private List<Composition> _Compositions;
        private List<Order> _Orders;
        public DbInitializer(CafeDB dB, ILogger<DbInitializer> logger)
        {
            _db = dB;
            _Logger = logger;
        }

        public void Initialize()
        {
            var timer = Stopwatch.StartNew();

            _Logger.LogInformation("Инициализация БД...");
            if (((RelationalDatabaseCreator)_db.GetService<IDatabaseCreator>()).Exists() && _db.Dishes.Any()) return;

            _Logger.LogInformation("Удаление существующей БД...");
            _db.Database.EnsureDeleted();
            _Logger.LogInformation("Удаление существующей БД выполнено за {0} мс", timer.ElapsedMilliseconds);

            _Logger.LogInformation("Миграция БД...");
            _db.Database.Migrate();
            _Logger.LogInformation("Миграция БД выполнена за {0} мс", timer.ElapsedMilliseconds);

            if (!IsDishListCorrect()) throw new ArgumentException("Список блюд содержит неизвестные категории или продукты в составе!");
            if (!IsOrderListCorrect()) throw new ArgumentException("Список заказов содержит неизвестные блюда в составе (или нарушены прочие ограничения)!");

            _Logger.LogInformation("Заполнение БД первичными данными...");
            InitializeData();
            _Logger.LogInformation("Заполнение БД первичными данными выполнено за {0} мс", timer.ElapsedMilliseconds);
            _Logger.LogInformation("Инициализация БД выполнена за {0} сек", timer.Elapsed.TotalSeconds);
        }

        private void InitializeData()
        {
            InitializeProduct();
            InitializeCategory();
            InitializeDish();
            InitializeComposition();
            InitializeOrder();
        }

        /// <summary>
        /// Проверка корректности стартовых данных в заказах
        /// </summary>
        /// <returns></returns>
        private bool IsOrderListCorrect()
        {
            /* **Прочие ограничения, заданные в условиях проекта** */
            var maxdish = 6; // Максимальное количество блюд в заказе
            var maxQty = 2; // Максимальное количество блюд одного вида в одном заказе

            foreach (var order in _OrdersList)
            {
                foreach (var dish_qty in order.Value.dishes_qty)
                {
                    if (!_DishesList.ContainsKey(dish_qty.Key)) return false; //Проверяем соответствие блюд в заказе блюдам в меню (списке блюд)
                    if (dish_qty.Value > maxQty) return false; //Проверяем максимальное количество блюд одного вида в одном заказе
                }
                if (order.Value.dishes_qty.Count > maxdish) return false; //Проверяем максимальное количество блюд в заказе
            }
            return true;
        }


        /// <summary>
        /// Проверка корректности стартовых данных в блюдах
        /// </summary>
        /// <returns></returns>
        private bool IsDishListCorrect()
        {
            foreach (var dish in _DishesList)
            {
                if (!_CategoriesList.Contains(dish.Value.category))
                    return false; //Проверяем что категории блюд есть в списке категорий
                if (dish.Value.components.Any(component => !_ProductsList.ContainsKey(component)))
                    return false; //Проверяем что состав блюда есть в списке продуктов
            }
            return true;
        }
        private void InitializeProduct()
        {
            var count = _ProductsList.Count;
            _Products = new Product[count];
            var i = 0;
            foreach (var product in _ProductsList)
            {
                _Products[i++] = new Product { Name = product.Key, Remains = product.Value };
            }
            _db.Products.AddRange(_Products);
            _db.SaveChanges();
        }
        private void InitializeCategory()
        {
            var count = _CategoriesList.Length;
            _Categories = new Category[count];
            for (int i = 0; i < count; i++)
            {
                _Categories[i] = new Category { Name = _CategoriesList[i] };
            }
            _db.Categories.AddRange(_Categories);
            _db.SaveChanges();
        }
        private void InitializeDish()
        {
            var count = _DishesList.Count;
            var i = 0;
            _Dishes = new Dish[count];
            foreach (var dish in _DishesList)
            {
                _Dishes[i++] = new Dish
                {
                    Name = dish.Key,
                    Weight = dish.Value.weigth,
                    Price = dish.Value.price,
                    IsActive = true,
                    CategoryId = _db.Categories.Where(x => x.Name == dish.Value.category).Select(x => x.Id).FirstOrDefault()
                };
            }
            _db.Dishes.AddRange(_Dishes);
            _db.SaveChanges();
        }
        private void InitializeComposition()
        {
            _Compositions = new List<Composition>();
            foreach (var dish in _DishesList)
            {
                for (int i = 0; i < dish.Value.components.Count; i++)
                {
                    _Compositions.Add(new Composition()
                    {
                        DishId = _db.Dishes.Where(x => x.Name == dish.Key).Select(x => x.Id).FirstOrDefault(),
                        ProductId = _db.Products.Where(x => x.Name == dish.Value.components[i]).Select(x => x.Id).FirstOrDefault()
                    });
                }
            }
            //Вставить данные в эту таблицу по-другому не удалось... странное поведение EF Core
            foreach (var composition in _Compositions)
            {
                _db.Database.ExecuteSqlRaw($@"INSERT INTO composition (Dish_Id, Product_Id) VALUES ({composition.DishId},{composition.ProductId})");
            }
        }

        private void InitializeOrder()
        {
            _Orders = new List<Order>();
            foreach (var order in _OrdersList)
            {
                foreach (var dish_qty in order.Value.dishes_qty)
                {
                    _Orders.Add(new Order()
                    {
                        OrderNumber = order.Key,
                        OrderDate = DateTime.Parse(order.Value.data),
                        Qty = dish_qty.Value,
                        DishId = _db.Dishes.Where(x => x.Name == dish_qty.Key).Select(x => x.Id).FirstOrDefault()
                    });

                }
            }
            _db.Orders.AddRange(_Orders);
            _db.SaveChanges();
        }
    }
}
