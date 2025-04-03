namespace SurveyBasket.Api.Contracts.Votes;

public record VotesPerDayResponse(
    DateOnly Date,
    int NumberOfVotes
);
