namespace SurveyBasket.Api.Contracts.Votes;

public record VotesPerAnswerResponse
(
    string Answer,
    int Count
);
