namespace EventManagmentSystem.ResultObject;

public class ResultObject<T>
{
    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }
    public T? Value { get; private set; }

    public ResultObject<T> Success(T? value, string message="")
    {
        ResultObject < T > resultObject = new ResultObject<T>
        {
            IsSuccess = true,
            Value = value,
            Message = message
        };
        return resultObject;
    }

    public ResultObject<T> Failure(string message)
    {
        ResultObject<T> resultObject = new ResultObject<T>
        {
            IsSuccess = false,
            Message = message
        };
        return resultObject;
    }
}