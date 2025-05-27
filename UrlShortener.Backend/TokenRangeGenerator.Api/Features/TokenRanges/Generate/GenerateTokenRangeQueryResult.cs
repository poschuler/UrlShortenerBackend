namespace TokenRangeGenerator.Api.Features.TokenRanges.Generate
{
    public sealed class GenerateTokenRangeQueryResult
    {
        public long? AllocatedRangeStart { get; set; }
        public long? AllocatedRangeEnd { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
