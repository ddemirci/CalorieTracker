using System.Threading.Tasks;
using CalorieTracker.Data;

namespace CalorieTracker.Services
{
    public class BaseService
    {
        protected readonly DataContext DataContext;
        protected BaseService(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await DataContext.SaveChangesAsync() > 0;
        }
    }
}