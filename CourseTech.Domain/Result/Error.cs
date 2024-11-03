namespace CourseTech.Domain.Result;

/// <summary>
/// Ошибка, которая создаётся в BaseResult
/// </summary>
public class Error
{
    public Error(string errorMessage, int? errorCode)
    {
        Message = errorMessage;
        Code = errorCode;
    }

    public Error() { }

    public string Message { get; }

    public int? Code { get; }
}
