namespace SurveyBasket.Api.Contracts.Votes;

public record VotesPerQuestionResponse(
    string Question,
    IEnumerable<VotesPerAnswerResponse> SelectedAnswers
    );
