namespace SurveyBasket.Api.Contracts.Votes;

public class VoteAnswerRequestValidator : AbstractValidator<VoteAnswerRequest>
{
    public VoteAnswerRequestValidator()
    {
        RuleFor(x => x.AnswerID)
            .GreaterThan(0);
        RuleFor(x => x.QuestionId)
            .GreaterThan(0);
    }
}
