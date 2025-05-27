using FluentValidation;

namespace TokenRangeGenerator.Api.Features.TokenRanges.Generate
{
    public class GenerateTokenRangeValidator : AbstractValidator<GenerateTokenRangeCommand>
    {
        public GenerateTokenRangeValidator()
        {
            RuleFor(c => c.Size)
                    .NotNull()
                    .GreaterThan(0)
                    .LessThanOrEqualTo(10000);

            RuleFor(c => c.RequestServerId)
                .NotEmpty()
                .MaximumLength(100);
        }

    }
}
