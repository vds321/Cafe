using Microsoft.EntityFrameworkCore;
using Storage.Context;
using Storage.Entities;
using System.Linq;

namespace Storage
{
    class OrdersRepository : DbRepository<Order>
    {
        public OrdersRepository(CafeDB db) : base(db) { }

        #region Overrides of DbRepository<Order>

        public override IQueryable<Order> Items => base.Items.Include(items => items.Dish);

        #endregion
    }
}