using Microsoft.AspNetCore.Identity.Data;

namespace SurveyBasket.Api.Contracts.Poll;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty();
    }

}
