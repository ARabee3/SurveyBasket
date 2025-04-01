using SurveyBasket.Api.Abstractions;

namespace SurveyBasket.Api.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound =
        new Error("Poll.NotFound", "No Poll Found Associated With Given ID");
}
