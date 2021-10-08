using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Storage.Entities;

#nullable disable

namespace Storage.Context
{
    public partial class CafeDB : DbContext
    {
        public CafeDB()
        {
        }

        public CafeDB(DbContextOptions<CafeDB> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Composition> Compositions { get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
    }
}
