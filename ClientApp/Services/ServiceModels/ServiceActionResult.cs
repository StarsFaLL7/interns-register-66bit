namespace ClientApp.Services.ServiceModels;

/// <summary>
/// Результат выполнения действия сервиса
/// </summary>
/// <typeparam name="T">Тип, возвращаемый в свойстве Item</typeparam>
public class ServiceActionResult<T> where T : class
{
    /// <summary>
    /// В случае успешной отработки, когда ожидается ответ в виде объекта, используется данное свойство.
    /// </summary>
    public T? Item { get; set; }
    
    /// <summary>
    /// Индикатор, успешно ли выполнено запрашиваемое действие.
    /// </summary>
    public bool Completed { get; set; }
    
    /// <summary>
    /// В случае ошибки во время выполнения действия, комментарий сохраняется в данном поле.
    /// </summary>
    public string? Message { get; set; }
}