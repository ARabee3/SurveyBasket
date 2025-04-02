using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Questions;
using SurveyBasket.Api.Contracts.Votes;
using SurveyBasket.Api.Entities;
using SurveyBasket.Api.Errors;

namespace SurveyBasket.Api.Services;

public class VoteService(ApplicationDbContext context) : IVoteService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default)
    {       
        
        var hasVote = await _context.Votes.AnyAsync(x => x.PollId == pollId && x.UserId == userId, cancellationToken);

        if (hasVote)
            return Result.Failure(VoteErrors.DuplicatedVote);
        var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId && x.IsPublished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken: cancellationToken);
        if (!pollIsExists)
            return Result.Failure(PollErrors.PollNotFound);

        var availableQuestions = await _context.Questions
                                    .Where(q => q.PollId == pollId && q.IsActive)
                                    .Select(q =>  q.Id )
                                    .ToListAsync(cancellationToken);

        if (!request.Answers.Select(x => x.QuestionId).SequenceEqual(availableQuestions)) { return Result.Failure(VoteErrors.InvalidQuestions); }

        var vote = new Vote()
        {
            PollId = pollId,
            UserId = userId,
            VoteAnswers = request.Answers.Select(a => new VoteAnswer
            {
                QuestionId = a.QuestionId,
                AnswerId = a.AnswerID
                // VoteId will be set by EF Core relationship fixup
            }).ToList()
        };

        await _context.AddAsync(vote, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
