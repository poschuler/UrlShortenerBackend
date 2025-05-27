namespace TokenRangeGenerator.Api.Features.TokenRanges.Generate
{
    public sealed class GenerateTokenRangeResponse
    {
        public required long RangeStart { get; set; }
        public required long RangeEnd { get; set; }
    }
}
