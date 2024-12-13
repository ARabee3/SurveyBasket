namespace SurveyBasket.Api.Services;

public class PollService : IPollService
{
    private static readonly List<Poll> _polls = [
       new Poll {
            Id = 1,
            Title = "Test",
            Description = "Loreum Ipsum"

        } ]; // new in version .net 8 ((empty list))
    public IEnumerable<Poll> GetAll() => _polls;
    public Poll? Get(int Id) => _polls.SingleOrDefault(x => x.Id == Id);

    public Poll Add(Poll request)
    {
        request.Id = _polls.Count + 1;
        _polls.Add(request);
        return request;
    }
    public bool Update(int id, Poll poll)
    {
        var current = Get(id);
        if (current is null) return false;
        current.Title = poll.Title;
        current.Description = poll.Description;
        return true;
    }

    public bool Delete(int id)
    {
        var current = Get(id);
        if (current is null) return false;
        _polls.Remove(current);
        return true;
    }
}
