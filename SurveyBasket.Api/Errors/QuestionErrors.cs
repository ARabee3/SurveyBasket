using SurveyBasket.Api.Abstractions;

namespace SurveyBasket.Api.Errors;

public static class QuestionErrors
{
    public static readonly Error QuestionNotFound =
        new Error("Question.NotFound", "No Question Found Associated With Given ID");

    public static readonly Error DuplicatedQuestionContent =
        new Error("Question.DuplicatedContent", "Another Question Found With The Same Content");


}
