using SurveyBasket.Api.Abstractions;

namespace SurveyBasket.Api.Errors;

public static class VoteErrors
{
    public static readonly Error DuplicatedVote =
        new("Vote.DuplicatedVote", "This User Already voted before");
    public static readonly Error InvalidQuestions =
        new("Vote.InvalidQuestions", "Invalid Questions");
}
