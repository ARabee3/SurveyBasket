using SurveyBasket.Api.Models;
using System.Data;

namespace SurveyBasket.Api.Services;

public interface IPollService
{
    IEnumerable<Poll> GetAll();
    Poll? Get(int Id);
    Poll Add(Poll request);
    bool Update(int id, Poll poll);
    bool Delete(int id);

}
