using System.Collections.Generic;

namespace Cafe.Data
{
    internal static class DbStartDataInitialize
    {
        public static readonly string[] CategoriesList = { "Первое", "Горячее", "Напитки", "Десерт" };
        public static readonly Dictionary<string, double> ProductsList = new()
        {
            { "Свекла", 34 },
            { "Лук", 26 },
            { "Картофель", 65 },
            { "Масло", 11 },
            { "Клюква", 10 },
            { "Сахар", 5 },
            { "Говядина", 7 },
            { "Молоко", 3 },
            { "Свинина", 13 },
            { "Горох", 1 },
            { "Чай весовой", 2 },
            { "Морковь", 1 },
            { "Помидоры", 1.5 },
            { "Кабачки", 5 }
        };
        public static readonly Dictionary<string, (double weigth, decimal price, string category, List<string> components)> DishesList = new()
        {
            { "Борщ", (140, 68, "Первое", new List<string> { "Свекла", "Лук", "Картофель", "Говядина" }) },
            { "Картофельное пюре", (150, 47, "Горячее", new List<string> { "Картофель", "Масло" }) },
            { "Клюквенный морс", (180, 30, "Напитки", new List<string> { "Клюква", "Сахар" }) },
            { "Гороховый суп", (250, 55, "Первое", new List<string> { "Горох", "Лук", "Свинина", "Картофель" }) },
            { "Рагу овощное", (150, 45, "Горячее", new List<string> { "Кабачки", "Помидоры", "Морковь" }) },
            { "Чай", (200, 35, "Напитки", new List<string> { "Чай весовой", "Сахар" }) },
            { "Мороженое", (100, 75, "Десерт", new List<string> { "Молоко", "Сахар" }) }
        };
        public static readonly Dictionary<int, (string data, Dictionary<string, int> dishes_qty)> OrdersList = new()
        {
            { 1, ("2021-02-23 13:07", new Dictionary<string, int>() { { "Борщ", 1 }, { "Картофельное пюре", 2 } }) },
            { 2, ("2021-02-25 15:34", new Dictionary<string, int>() { { "Картофельное пюре", 2 } }) },
            { 3, ("2021-02-25 16:07", new Dictionary<string, int>() { { "Клюквенный морс", 2 }, { "Картофельное пюре", 2 } }) },
            { 4, ("2021-02-26 12:00", new Dictionary<string, int>() { { "Борщ", 2 }, { "Картофельное пюре", 2 }, { "Клюквенный морс", 2 }, { "Мороженое", 1 } }) }
        };
    }
}
