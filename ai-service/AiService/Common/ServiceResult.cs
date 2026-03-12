namespace AiService.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; private set; }
        public T? Data { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;
        public int StatusCode { get; private set; }

        public static ServiceResult<T> Ok(T data, int statusCode = 200)
        {
            return new ServiceResult<T>
            {
                Success = true,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ServiceResult<T> Fail(string errorMessage, int statusCode)
        {
            return new ServiceResult<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }
    }
}