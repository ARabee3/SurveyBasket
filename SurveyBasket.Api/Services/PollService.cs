using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using SurveyBasket.Api.Persistence;
using System.Threading;

namespace SurveyBasket.Api.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;

 
    public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);
    public async Task<Poll?> GetAsync(int Id, CancellationToken cancellationToken = default) =>
        await _context.Polls.FindAsync(Id, cancellationToken);

    public async Task<Poll> AddAsync(Poll request, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(request, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return request;
    }
    public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken = default)
    {
        var current = await GetAsync(id, cancellationToken);
        if (current is null) return false;
        current.Title = poll.Title;
        current.Summary = poll.Summary; 
        current.StartsAt = poll.StartsAt;
        current.EndsAt = poll.EndsAt;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var current = await GetAsync(id, cancellationToken);
        if (current is null) return false;
        _context.Remove(current);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken)
    {
        var current = await GetAsync(id, cancellationToken);
        if (current is null) return false;

        current.IsPublished = !current.IsPublished;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
