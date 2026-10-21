namespace SchoolHub.Common
{
    public class Result
    {
        public bool Succeeded { get; private set; }
        public string? Error { get; private set; }

        private Result(bool succeeded, string? error)
        {
            Succeeded = succeeded;
            Error = error;
        }

        public static Result Success() => new(true, null);

        public static Result Failure(string error) => new(false, error);
    }

    public class Result<T>
    {
        public bool Succeeded { get; private set; }
        public T? Value { get; set; }
        public string? Error { get; private set; }

        private Result(bool succeeded, T? value, string? error)
        {
            Succeeded = succeeded;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new(true, value, null);

        public static Result<T> Failure(string error) => new(false, default, error);
    }
}