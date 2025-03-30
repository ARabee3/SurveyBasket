using Microsoft.AspNetCore.Identity.Data;

namespace SurveyBasket.Api.Contracts.Authentication;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty(); 
        
        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.Email).EmailAddress();
    }

   
}
