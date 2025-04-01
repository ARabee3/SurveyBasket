using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Persistence;
using System.Threading;

namespace SurveyBasket.Api.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;

 
    public async Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Polls
               .AsNoTracking()
               .ProjectToType<PollResponse>() 
               .ToListAsync(cancellationToken);
    }
    public async Task<IEnumerable<PollResponse>> GetCurrentAsync(CancellationToken cancellationToken = default) =>
        await _context.Polls
        .Where(x=> x.IsPublished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
        .AsNoTracking()
        .ProjectToType<PollResponse>()
        .ToListAsync(cancellationToken);

    public async Task<Result<PollResponse>> GetAsync(int Id, CancellationToken cancellationToken = default)
    {
        var result = await _context.Polls.FindAsync(Id, cancellationToken);
        return result is not null 
            ? Result.Success(result.Adapt<PollResponse>()) 
            : Result.Failure<PollResponse>(PollErrors.PollNotFound);
    }

    public async Task<PollResponse> AddAsync(PollRequest request, CancellationToken cancellationToken = default)
    {
        
        var pollEntity = request.Adapt<Poll>();
        await _context.AddAsync(pollEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return pollEntity.Adapt<PollResponse>();
    }
    public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default)
    {
        var current = await _context.Polls.FindAsync(id, cancellationToken); 
        if (current is null) 
            return Result.Failure(PollErrors.PollNotFound);

        current.Title = poll.Title;
        current.Summary = poll.Summary;
        current.StartsAt = poll.StartsAt;
        current.EndsAt = poll.EndsAt;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var current = await _context.Polls.FindAsync(id, cancellationToken);
        if (current is null)
            return Result.Failure(PollErrors.PollNotFound);
        _context.Remove(current);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken)
    {
        var current = await _context.Polls.FindAsync(id, cancellationToken);
        if (current is null)
            return Result.Failure(PollErrors.PollNotFound);

        current.IsPublished = !current.IsPublished;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
