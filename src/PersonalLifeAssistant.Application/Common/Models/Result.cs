namespace PersonalLifeAssistant.Application.Common.Models;

public class Result<T>
{
    public bool Succeeded { get; init; }
    public string Message { get; init; } = string.Empty;
    public T? Data { get; init; }

    public static Result<T> Success(T data, string message = "") => new()
    {
        Succeeded = true,
        Data = data,
        Message = message
    };

    public static Result<T> Failure(string message) => new()
    {
        Succeeded = false,
        Message = message
    };
}
