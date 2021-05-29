using System.Threading.Tasks;

namespace CalorieTracker.Services.IServices
{
    public interface IBaseService
    {
        Task<bool> SaveAllAsync();
    }
}