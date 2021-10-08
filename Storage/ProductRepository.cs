using Microsoft.EntityFrameworkCore;
using Storage.Context;
using Storage.Entities;
using System.Linq;

namespace Storage
{
    class ProductRepository : DbRepository<Product>
    {
        public ProductRepository(CafeDB db) : base(db) { }

        #region Overrides of DbRepository<Product>

        public override IQueryable<Product> Items => base.Items.Include(items => items.Composition);

        #endregion
    }
}
