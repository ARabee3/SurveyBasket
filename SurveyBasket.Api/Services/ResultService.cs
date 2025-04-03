using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Results;
using SurveyBasket.Api.Contracts.Votes;
using SurveyBasket.Api.Errors;
using System.Linq;
using System.Text.RegularExpressions;

namespace SurveyBasket.Api.Services;

public class ResultService(ApplicationDbContext context) : IResultService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default)
    {

        var pollVotes = await _context.Polls
                        .Where(x => x.Id == pollId)
                        .Select(x => new PollVotesResponse(
                            x.Title,
                            x.Votes.Select(v => new VoteResponse(
                                    $"{ v.User.FirstName } {v.User.LastName}",
                                    v.SubmittedOn,
                                    v.VoteAnswers.Select(a => new QuestionAnswerResponse(
                                        a.Question.Content,
                                        a.Answer.Content
                                        ))
                                ))
                            ))
                        .SingleOrDefaultAsync(cancellationToken);
        return pollVotes is null ? Result.Failure<PollVotesResponse>(PollErrors.PollNotFound)
                                 : Result.Success(pollVotes);
    }

    public async Task<Result<List<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);
        if (!pollIsExists)
            return Result.Failure<List<VotesPerDayResponse>>(PollErrors.PollNotFound);

        var votesPerDay = await _context.Polls
                                .Where(p => p.Id == pollId)
                                .SelectMany(x => x.Votes)
                                .GroupBy(v => DateOnly.FromDateTime(v.SubmittedOn))
                                .Select(vp => new VotesPerDayResponse(
                                        vp.Key,
                                        vp.Count()
                                    ))
                                .ToListAsync(cancellationToken);
        return Result.Success(votesPerDay);
    }

    public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);
        if (!pollIsExists)
            return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.PollNotFound);

        var votesPerQuestion = await _context.VoteAnswers
                               .Where(va => va.Vote.PollId == pollId)
                               .Select(x => new VotesPerQuestionResponse(
                                   x.Question.Content,
                                   x.Question.Votes
                                   .GroupBy(x => new { AnswerId= x.Answer.Id, AnswerContent = x.Answer.Content } )
                                   .Select(g => new VotesPerAnswerResponse(
                                       g.Key.AnswerContent,
                                       g.Count()
                                       ))
                                   ))
                               .ToListAsync(cancellationToken);
        return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesPerQuestion);

    }
}
