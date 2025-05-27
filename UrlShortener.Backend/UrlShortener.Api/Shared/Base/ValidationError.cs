namespace UrlShortener.Api.Shared.Base
{
    public class ValidationError : Error
    {
        public Error[] Errors { get; }

        public ValidationError(Error[] errors)
            : base(
                  "Validation",
                  "One or more validation errors occurred",
                  ErrorType.Validation)
        {
            Errors = errors;
        }

        public static ValidationError FromResults(IEnumerable<Result> results) =>
            new(results.Where(r => r.IsFailure).Select(r => r.Error).ToArray());

    }
}
