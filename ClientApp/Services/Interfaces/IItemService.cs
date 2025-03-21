using ClientApp.Services.ServiceModels;

namespace ClientApp.Services.Interfaces;

/// <summary>
/// Интерфейс для CRUD операций над сущностями
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public interface IItemService<T> where T : class
{
    
    /// <summary>
    /// Получить список всех сущностей.
    /// </summary>
    public Task<ServiceActionResult<List<T>>> GetAllAsync();
    
    /// <summary>
    /// Получить отфильтрованный и отсортированный список всех сущностей, используя пагинацию.
    /// </summary>
    public Task<ServiceActionResult<FilteredList<T>>> GetFilteredAsync(FilteredListRequest query);
    
    /// <summary>
    /// Получить экземпляр объекта с заполненными полями.
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности</param>
    public Task<ServiceActionResult<T>> GetAggregatedByIdAsync(Guid id);
    
    /// <summary>
    /// Сохранить изменения, выполненные над сущностью.
    /// </summary>
    /// <param name="item">Сущность</param>
    public Task<ServiceActionResult<T>> SaveAsync(T item);
    
    /// <summary>
    /// Добавить новую сущность.
    /// </summary>
    /// <param name="item">Сущность</param>
    public Task<ServiceActionResult<T>> CreateAsync(T item);
    
    /// <summary>
    /// Удалить существующую сущность.
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности</param>
    public Task<ServiceActionResult<T>> DeleteAsync(Guid id);
}