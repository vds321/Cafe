using System.Collections.Generic;
using System.Linq;

namespace Storage.Interface
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        /// <summary>
        /// Получить коллекцию объектов
        /// </summary>
        IQueryable<T> Items { get; }

        #region Синхронные операции с репозиториями

        /// <summary>
        /// Получить объект по его идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(int id);

        /// <summary>
        /// Сохранить объект в БД
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Возвращает сохраненный объект</returns>
        T Add(T item);

        /// <summary>
        /// Сохранить коллекцию объектов в БД
        /// </summary>
        /// <param name="itemCollection"></param>
        /// <returns>Возвращает количество сохраненных объектов</returns>
        int AddRange(IEnumerable<T> itemCollection);

        /// <summary>
        /// Изменить объект в БД
        /// </summary>
        /// <param name="item"></param>
        void Update(T item);

        /// <summary>
        /// Удалить объект из БД
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);


        /// <summary>
        /// Выполняет запрос в БД и возвращает результат
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Возвращает коллекцию объектов</returns>
        IQueryable<T> ExecuteSQL(string query);

        #endregion

        #region Асинхронные операции с репозиториями

        /*
        Task<T> GetAsync(int id, CancellationToken Cancel = default);

        Task<T> AddAsync(T item, CancellationToken Cancel = default);

        Task UpdateAsync(T item, CancellationToken Cancel = default);

        Task RemoveAsync(int id, CancellationToken Cancel = default);
        */

        #endregion
    }
}
