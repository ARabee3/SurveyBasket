using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Results;
using SurveyBasket.Api.Contracts.Votes;

namespace SurveyBasket.Api.Services;

public interface IResultService
{
    public Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default);
    public Task<Result<List<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default);
    public Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default);
}
