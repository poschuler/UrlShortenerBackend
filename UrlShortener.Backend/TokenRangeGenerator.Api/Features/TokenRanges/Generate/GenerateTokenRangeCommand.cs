using TokenRangeGenerator.Api.Shared.Messaging;

namespace TokenRangeGenerator.Api.Features.TokenRanges.Generate
{
    public sealed record GenerateTokenRangeCommand(int Size, string RequestServerId) : ICommand<GenerateTokenRangeResponse>;
}
