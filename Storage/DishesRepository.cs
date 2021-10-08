using Microsoft.EntityFrameworkCore;
using Storage.Context;
using Storage.Entities;
using System.Linq;

namespace Storage
{
    class DishesRepository : DbRepository<Dish>
    {
        public DishesRepository(CafeDB db) : base(db) { }

        #region Overrides of DbRepository<Dish>

        public override IQueryable<Dish> Items => base.Items.Include(item => item.Category)
                                                            .Include(item => item.Compositions)
                                                            .Include(item => item.Orders)
                                                            ;

        #endregion
    }
}
