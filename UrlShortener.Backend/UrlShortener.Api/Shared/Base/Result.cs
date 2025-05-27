using System.Diagnostics.CodeAnalysis;

namespace UrlShortener.Api.Shared.Base
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public bool IsRedirect => RedirectUrl is not null;

        public string? RedirectUrl { get; }
        public bool IsPermanentRedirect { get; }

        public Error Error { get; }


        public Result(bool isSuccess, Error error, string? redirectUrl = null, bool isPermanentRedirect = false)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Not valid error object", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
            RedirectUrl = redirectUrl;
            IsPermanentRedirect = isPermanentRedirect;
        }

        public static Result Success() => new(true, Error.None);

        public static Result<TValue> Success<TValue>(TValue value)
            => new(value, true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Failure<TValue>(Error error)
            => new(default, false, error);

        public static Result Redirect(string url, bool permanent = true)
            => new(true, Error.None, url, permanent);

    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        [NotNull]
        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result can't be accessed.");

        public static implicit operator Result<TValue>(TValue? value) =>
            value is not null
                ? Success(value)
                : Failure<TValue>(Error.NullValue);

        public static Result<TValue> ValidationFailure(Error error) =>
            new(default, false, error);
    }
}