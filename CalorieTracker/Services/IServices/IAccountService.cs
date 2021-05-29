using System.Threading.Tasks;
using CalorieTracker.Models;

namespace CalorieTracker.Services.IServices
{
    public interface IAccountService : IBaseService
    {
        Task<AppUser> GetUserByUserName(string userName);
    }
}