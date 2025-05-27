using UrlShortener.Api.Shared.Base;

namespace UrlShortener.Api.Shared.Messaging
{
    public interface IQueryHandler<in TQuery>
        where TQuery : IQuery
    {
        Task<Result> Handle(TQuery query, CancellationToken cancellationToken);
    }

    public interface IQueryHandler<in TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
