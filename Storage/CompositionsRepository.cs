using Microsoft.EntityFrameworkCore;
using Storage.Context;
using Storage.Entities;
using System.Linq;

namespace Storage
{
    class CompositionsRepository : DbRepository<Composition>
    {
        public CompositionsRepository(CafeDB db) : base(db) { }

        #region Overrides of DbRepository<Composition>

        public override IQueryable<Composition> Items => base.Items.Include(items => items.Dish)
                                                                   .Include(items => items.Product);

        #endregion
    }
}
