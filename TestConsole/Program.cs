using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Storage.Context;
using Storage.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;


namespace TestConsole
{
    public static class Program
    {
        static void Main(string[] args)
        {

            const string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=CAFE;Integrated Security=True";
            const string connection1 = "Server=192.168.1.105:3307;user=app;password=App_password1;database=CAFE";

            using var db = new CafeDB(new DbContextOptionsBuilder<CafeDB>().UseMySql(connection1, new MariaDbServerVersion("10.3.27")).Options);

            //var query = db.Dishes.Join(db.Categories, d => d.CategoryId, c => c.Id,
            //    (d, c) => new Dish()
            //    {
            //        Id = d.Id,
            //        Name = d.Name,
            //        Weight = d.Weight,
            //        Price = d.Price,
            //        Category = c
            //    });

            //var q = query.ToQueryString();

            //foreach (var dish in query)
            //{
            //    Console.WriteLine($"{dish.Id}, {dish.Name}, {dish.Weight}, {dish.Price}, {dish.Category.Name}");
            //}


            var comp1 = "Картофель";
            var comp2 = "Лук";

            var comp = db.Dishes.Join(db.Compositions, d => d.Id, c => c.DishId, (d, c) => new { d.Name, c.ProductId })
                                .Join(db.Products, arg => arg.ProductId, p => p.Id, (__, p) => new { DishName = __.Name, ProdName = p.Name })
                                .AsEnumerable()
                                .GroupBy(d => d.DishName, d => d.ProdName)
                                .ToDictionary(k => k.Key, i => i.ToList())
                                .Where(x => x.Value.Any(y => y.Equals(comp1)))
                                .Where(x => x.Value.Any(y => y.Equals(comp2)))
                                .Select(x => x.Key);


            //.Select(x => new { dishName = x.Key, prodList = x.Value.Any(y => y.Contains(comp2) && y.Contains(comp3)) });
            //.Select(d => new { DishName = d.Key, ProdCount = d.Count() });

            //var q1 = comp.ToQueryString();


            foreach (var obj in comp)
            {
                Console.WriteLine($"{obj}");

            }



            Console.ReadLine();
        }
    }
}
