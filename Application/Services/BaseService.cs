using System.Linq.Dynamic.Core;
using Domain.DataQuery;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Services;

/// <summary>
/// Класс, реализующий взаимодействие с базой данной для любой сущности, реализующей интерфейс IHasId.
/// </summary>
/// <typeparam name="TEntity">Сущность из базы данных</typeparam>
public class BaseService<TEntity> where TEntity : class, IHasId
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public BaseService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    /// <summary>
    /// Выполнить запрос на получение объектов к базе данных с указанным параметрами запроса.
    /// </summary>
    /// <returns>
    /// Возвращается массив объектов, которые соответствуют запросу.
    /// </returns>
    public virtual async Task<TEntity[]> GetAsync(DataQueryParams<TEntity> queryParams)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>().AsQueryable();
        if (queryParams.Expression != null)
        {
            set = set.Where(queryParams.Expression);
        }

        if (queryParams.Filters != null)
        {
            foreach (var filter in queryParams.Filters)
            {
                set = set.Where(filter);
            }
        }
        
        if (queryParams.Sorting != null)
        {
            if (queryParams.Sorting.OrderBy == null)
            {
                if (queryParams.Sorting.PropertyName != null)
                {
                    set = queryParams.Sorting.Ascending ? set.OrderBy(queryParams.Sorting.PropertyName) : 
                        set.OrderBy(queryParams.Sorting.PropertyName + " descending");
                }
            }
            else
            {
                if (queryParams.Sorting.ThenBy != null)
                {
                    set = queryParams.Sorting.Ascending ? 
                        set.OrderBy(queryParams.Sorting.OrderBy).ThenBy(queryParams.Sorting.ThenBy) : 
                        set.OrderByDescending(queryParams.Sorting.OrderBy).ThenByDescending(queryParams.Sorting.ThenBy);
                }
                else
                {
                    set = queryParams.Sorting.Ascending ? 
                        set.OrderBy(queryParams.Sorting.OrderBy) : 
                        set.OrderByDescending(queryParams.Sorting.OrderBy);
                }
            }
        }
        
        if (queryParams.Paging != null)
        {
            set = set.Skip(queryParams.Paging.Skip).Take(queryParams.Paging.Take);
        }
        
        set = ApplyIncludeParams(set, queryParams.IncludeParams);
        
        return set.ToArray();
    }

    /// <summary>
    /// Выполнить запрос на получение кол-ва объектов в базе данных с указанным параметрами запроса. Параметры Paging, Order и Include игнорируются.
    /// </summary>
    /// <returns>
    /// Возвращается кол-во объектов, которые соответствуют запросу.
    /// </returns>
    public virtual async Task<int> GetCountAsync(DataQueryParams<TEntity> queryParams)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>().AsQueryable();
        if (queryParams.Expression != null)
        {
            set = set.Where(queryParams.Expression);
        }

        if (queryParams.Filters != null)
        {
            foreach (var filter in queryParams.Filters)
            {
                set = set.Where(filter);
            }
        }
        
        return set.Count();
    }
    
    /// <summary>
    /// Получить объект из базы данных по конкретному идентификатору id
    /// </summary>
    /// <param name="id">Идентификатор объекта в базе данных.</param>
    /// <returns>
    /// Возвращается объект с указанным id, если он был найден в базе данных. Иначе - null.
    /// </returns>
    public virtual async Task<TEntity?> GetByIdOrDefaultAsync(Guid id)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>();
        return set.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Получить объект из базы данных по конкретному идентификатору id
    /// </summary>
    /// <param name="id">Идентификатор объекта в базе данных.</param>
    /// <param name="includeParams">Параметры для include полей из базы данных.</param>
    /// <returns>
    /// Возвращается объект с указанным id, если он был найден в базе данных. Иначе - null.
    /// </returns>
    public virtual async Task<TEntity?> GetByIdOrDefaultAsync(Guid id, IncludeParams<TEntity> includeParams)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>().AsQueryable();
        set = ApplyIncludeParams(set, includeParams);
        return set.FirstOrDefault(item => item.Id == id);
    }

    private IQueryable<TEntity> ApplyIncludeParams(IQueryable<TEntity> set, IncludeParams<TEntity>? includeParams)
    {
        if (includeParams != null)
        {
            if (includeParams.IncludeProperties != null)
            {
                foreach (var propertyPath in includeParams.IncludeProperties)
                {
                    set = set.Include(propertyPath);
                }
            }

            if (includeParams.IncludePropertiesPaths != null)
            {
                foreach (var propertyPath in includeParams.IncludePropertiesPaths)
                {
                    set = set.Include(propertyPath);
                }
            }
        }

        return set;
    }
    
    /// <summary>
    /// Удаляет сущность с указанным id из базы данных, если она существует.
    /// </summary>
    /// <param name="id">Идентификатор объекта в базе данных.</param>
    /// <returns>
    /// True - объект был найден и удален из базы данных.
    /// False - объект с указанным id не был найден в базе данных.
    /// </returns>
    public virtual async Task<bool> TryRemoveAsync(Guid id)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>();
        var item = set.FirstOrDefault(item => item.Id == id);
        if (item == null)
        {
            return false;
        }

        set.Remove(item);
        await dbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Удаляет сущности с указанными id из базы данных, если они существуют.
    /// </summary>
    /// <param name="ids">Идентификаторы объектов в базе данных.</param>
    public virtual async Task RemoveRangeAsync(params Guid[] ids)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>();
        var items = set.Where(item => ids.Contains(item.Id)).ToArray();
        set.RemoveRange(items);
        await dbContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// Удаляет сущности из базы данных.
    /// </summary>
    /// <param name="entities">Объекты в базе данных.</param>
    public virtual async Task RemoveRangeAsync(params TEntity[] entities)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>();
        set.RemoveRange(entities);
        await dbContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// Сохраняет сущность в соответствующую таблицу. Обновляет её поля, если она уже существует.
    /// </summary>
    public virtual async Task<Guid> SaveAsync(TEntity entity)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>();
        var existingItem = set.FirstOrDefault(item => item.Id == entity.Id);
        if (existingItem != null)
        {
            set.Remove(existingItem);
        }

        set.Add(entity);

        await dbContext.SaveChangesAsync();
        return entity.Id;
    }
}