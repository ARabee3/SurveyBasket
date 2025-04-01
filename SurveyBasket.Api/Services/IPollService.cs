using SurveyBasket.Api.Abstractions;
using System.Data;
using System.Threading;

namespace SurveyBasket.Api.Services;

public interface IPollService
{
    Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<PollResponse>> GetAsync(int Id, CancellationToken cancellationToken = default);
    Task<PollResponse> AddAsync(PollRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);
}
