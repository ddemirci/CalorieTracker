using System.Linq;
using System.Threading.Tasks;
using CalorieTracker.Data;
using CalorieTracker.Models;
using CalorieTracker.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CalorieTracker.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(DataContext context) 
            : base(context)
        {
        }

        public Task<AppUser> GetUserByUserName(string userName)
        {
            return DataContext.Users
                .AsQueryable()
                .Include(u => u.Meals)
                .ThenInclude(m => m.MealFoods)
                .Include(u => u.UserInformation)
                .Where(user => user.UserName == userName)
                .FirstOrDefaultAsync();
        }
    }
}