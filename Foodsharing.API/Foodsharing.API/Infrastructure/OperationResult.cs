namespace Foodsharing.API.Infrastructure;

/// <summary>
/// Результатов выполнения методов
/// </summary>
public class OperationResult
{
    /// <summary>
    /// Успешность выполнения
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// Сообщение о результате выполнения 
    /// </summary>
    public string? Message { get; set; }
    /// <summary>
    /// Данные, которые нужно передать в результате выполнения
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// Метод для формирования успешного результата выполнения
    /// </summary>
    /// <param name="message">Сообщение, которое нужно передать в результате выполнения</param>
    /// <param name="data">Данные, которые нужно передать в результате выполнения</param>
    /// <returns>Объект типа <see cref="OperationResult"/></returns>
    public static OperationResult SuccessResult(string message, string data = "")
    {
        return new OperationResult { Success = true, Message = message, Data = data };
    }

    /// <summary>
    /// Метод для формирования неуспешного результата выполнения
    /// </summary>
    /// <param name="message">Сообщение, которое нужно передать в результате выполнения</param>
    /// <returns>Объект типа <see cref="OperationResult"/></returns>
    public static OperationResult FailureResult(string message)
    {
        return new OperationResult { Success = false, Message = message };
    }
}
