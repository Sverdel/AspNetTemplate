using AspNetTemplate.Core.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace AspNetTemplate.Core.Validators
{
    [UsedImplicitly]
    public class HelloRequestValidator : AbstractValidator<HelloCommand>
    {
        public HelloRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Request).NotNull();
            RuleFor(x => x.Request.Name).NotEmpty();
        }
    }
}