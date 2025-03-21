using System.Linq.Expressions;
using Domain.Interfaces;

namespace Domain.DataQuery;

public class DataQueryParams<TEntity> where TEntity : class
{
    public Expression<Func<TEntity, bool>>? Expression { get; set; }
    
    public PagingParams? Paging { get; set; }
    
    public SortingParams<TEntity>? Sorting { get; set; }
    
    public List<Expression<Func<TEntity, bool>>>? Filters { get; set; }
    
    public IncludeParams<TEntity>? IncludeParams { get; set; }
}