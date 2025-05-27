using UrlShortener.Api.Shared.Base;

namespace UrlShortener.Api.Shared.Extensions
{
    public static class ResultExtensions
    {
        public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<Result, TOut> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result);
        }

        public static TOut Match<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, TOut> onSuccess,
            Func<Result<TIn>, TOut> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
        }

        public static IResult MatchOrRedirect(
        this Result result,
        Func<Result, IResult> onFailure,
        Func<string, bool, IResult> onRedirect)
        {
            if (result.IsRedirect && result.RedirectUrl is not null)
            {
                return onRedirect(result.RedirectUrl, result.IsPermanentRedirect);
            }

            return onFailure(result);
        }
    }
}
