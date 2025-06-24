namespace DAL.Contracts.DTO;

public class Response
{
    public bool Success { get; set; }
    public string Message { get; set; }

    public static Response Ok(string message = "Success") =>
        new() { Success = true, Message = message };

    public static Response Fail(string message) =>
        new() { Success = false, Message = message };
}

public class Response<T> : Response
{
    public T? Data { get; set; }

    public static Response<T> Ok(T data, string message = "Success") =>
        new() { Success = true, Message = message, Data = data };

    public new static Response<T> Fail(string message) =>
        new() { Success = false, Message = message, Data = default };
}