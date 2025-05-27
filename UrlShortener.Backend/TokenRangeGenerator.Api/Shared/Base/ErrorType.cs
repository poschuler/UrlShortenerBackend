using Microsoft.AspNetCore.Authentication;

namespace TokenRangeGenerator.Api.Shared.Base
{
    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        Problem = 2,
        NotFound = 3,
        Conflict = 4,
    }
}
