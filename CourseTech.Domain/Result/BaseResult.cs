namespace CourseTech.Domain.Result;

public class BaseResult
{
    protected BaseResult(Error error = null)
    {
        Error = error ?? new Error();
    }

    public bool IsSuccess => Error.Message == null;

    public Error Error { get; }

    public static BaseResult Success() => new BaseResult();

    public static BaseResult Failure(int errorCode, string errorMessage) =>
        new BaseResult(new Error(errorMessage, errorCode));
}

public class BaseResult<T> : BaseResult
{
    protected BaseResult(T data, Error error = null)
       : base(error)
    {
        Data = data;
    }

    public T Data { get; }

    public static BaseResult<T> Success(T data) =>
        new BaseResult<T>(data: data);

    public static new BaseResult<T> Failure(int errorCode, string errorMessage) =>
    new BaseResult<T>(default, new Error(errorMessage, errorCode));
}
