using Microsoft.EntityFrameworkCore;
using Storage.Context;
using Storage.Entities.Base;
using Storage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Storage
{

    internal abstract class DbRepository<T> : IRepository<T> where T : Entity, new()
    {

        private readonly CafeDB _db;
        private readonly DbSet<T> _Set;

        public bool AutoSaveChanges { get; set; } = true;

        public DbRepository(CafeDB db)
        {
            _db = db;
            _Set = db.Set<T>();
        }

        public virtual IQueryable<T> Items => _Set;

        public T Add(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Added;
            if (AutoSaveChanges) _db.SaveChanges();
            return item;
        }

        public int AddRange(IEnumerable<T> itemCollection)
        {
            int count = 0;
            foreach (T item in itemCollection)
            {
                if (item is null) throw new ArgumentNullException(nameof(item));
                _db.Entry(item).State = EntityState.Added;
                if (AutoSaveChanges) count += _db.SaveChanges();
            }
            return count;
        }


        public T Get(int id) => Items.SingleOrDefault(item => item.Id == id);

        public void Remove(int id)
        {
            var item = _Set.Local.FirstOrDefault(i => i.Id == id) ?? new T { Id = id };
            _db.Remove(item);
            if (AutoSaveChanges) _db.SaveChanges();
        }

        public void Update(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Modified;
            if (AutoSaveChanges) _db.SaveChanges();
        }

        public IQueryable<T> ExecuteSQL(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return (IQueryable<T>)Enumerable.Empty<T>();
            return _Set.FromSqlRaw(query);
        }

        #region Асинхронные операции
        /*
        public async Task<T> GetAsync(int id, CancellationToken Cancel = default) => await Items
            .SingleOrDefaultAsync(item => item.Id == id, Cancel)
            .ConfigureAwait(false);
        public async Task<T> AddAsync(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Added;
            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
            return item;
        }
        public async Task UpdateAsync(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Modified;
            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }
        public async Task RemoveAsync(int id, CancellationToken Cancel = default)
        {
            _db.Remove(new T { Id = id });
            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }
        */
        #endregion
    }
}
