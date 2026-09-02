using MyApi.Enums;

namespace MyApi.Common
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public ResultErrorType ErrorType { get; set; }
        public T? Data { get; set; }

        public static Result<T> Ok(T data)
        {
            return new Result<T>
            {
                Success = true,
                Data = data,
                ErrorType = ResultErrorType.None
            };
        }

        public static Result<T> Fail(string error, ResultErrorType errorType)
        {
            return new Result<T>
            {
                Success = false,
                Error = error,
                ErrorType = errorType
            };
        }
        

    }
}
