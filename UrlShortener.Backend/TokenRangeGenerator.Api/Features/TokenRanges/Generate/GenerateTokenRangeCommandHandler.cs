using Dapper;
using System.Data;
using System.Net;
using TokenRangeGenerator.Api.Shared.Base;
using TokenRangeGenerator.Api.Shared.Messaging;


namespace TokenRangeGenerator.Api.Features.TokenRanges.Generate
{
    public sealed class GenerateTokenRangeCommandHandler(IDbConnection dbConnection)
        : ICommandHandler<GenerateTokenRangeCommand, GenerateTokenRangeResponse>
    {
        public async Task<Result<GenerateTokenRangeResponse>> Handle(GenerateTokenRangeCommand command, CancellationToken cancellationToken)
        {
            var sql = "SELECT allocated_range_start as AllocatedRangeStart, allocated_range_end as AllocatedRangeEnd, message as Message FROM allocate_next_token_range(@ServerId, @RequestServerId, @RangeSize);";
            var parameters = new
            {
                ServerId = Dns.GetHostName(),
                RequestServerId = command.RequestServerId,
                RangeSize = command.Size
            };

            var result = await dbConnection.QueryFirstOrDefaultAsync<GenerateTokenRangeQueryResult>(sql, parameters);

            if (result is null)
            {
                return Result.Failure<GenerateTokenRangeResponse>(Error.Failure("TokenRange.InternalServerError", "Error generating token range"));
            }

            if (result.AllocatedRangeStart is null || result.AllocatedRangeEnd is null)
            {
                return Result.Failure<GenerateTokenRangeResponse>(Error.Problem("TokenRange.BadRequest", "Error generating token range"));
            }

            var response = new GenerateTokenRangeResponse
            {
                RangeStart = result.AllocatedRangeStart.Value,
                RangeEnd = result.AllocatedRangeEnd.Value,
            };

            return response;
        }
    }

}
