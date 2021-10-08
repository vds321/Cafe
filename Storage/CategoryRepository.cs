using Microsoft.EntityFrameworkCore;
using Storage.Context;
using Storage.Entities;
using System.Linq;

namespace Storage
{
    class CategoryRepository : DbRepository<Category>
    {
        public CategoryRepository(CafeDB db) : base(db) { }

        #region Overrides of DbRepository<Category>

        public override IQueryable<Category> Items => base.Items.Include(item => item.Dishes);

        #endregion
    }
}