using System.Linq.Expressions;

namespace Foodsharing.API.Interfaces;

/// <summary>
/// Интерфейс универсального репозитория
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public interface IRepository<T> : IDisposable
    where T : class
{
    /// <summary>
    /// Метод репозитория для получения всех сущностей из БД
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Коллекция всех сущностей</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод репозитория для получения сущности из БД по id
    /// </summary>
    /// <param name="id">id сущности</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Найденная сущность типа <typeparamref name="T"/> или null, если сущность не найдена</returns>
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод репозитория для добавления сущности в БД
    /// </summary>
    /// <param name="item">Добавляемая сущность</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Задача, представляющая асинхронную операцию</returns>
    Task AddAsync(T item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод репозитория для обновления сущности в БД
    /// </summary>
    /// <param name="item">Обновляемая сущность</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Задача, представляющая асинхронную операцию</returns>
    Task UpdateAsync(T item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод репозитория для удаления сущности из БД
    /// </summary>
    /// <param name="entity">Удаляемая сущность</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Задача, представляющая асинхронную операцию</returns>
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод репозитория для получения сущностей с включением связанных данных
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <param name="includeProperties">Массив выражений для включения связанных данных</param>
    /// <returns>Коллекция сущностей с включенными связанными данными</returns>
    Task<IEnumerable<T>> GetWithIncludeAsync(CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);

    /// <summary>
    /// Метод репозитория для получения сущностей с включением связанных данных по какому-то условию
    /// </summary>
    /// <param name="predicate">Условие для фильтрации сущностей</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <param name="includeProperties">Массив выражений для включения связанных данных</param>
    /// <returns>Коллекция сущностей, удовлетворяющих условию, с включенными связанными данными</returns>
    Task<IEnumerable<T>> GetWithIncludeAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties);
}
