using AutoMapper;
using CalorieTracker.Models;
using CalorieTracker.Models.ViewModels;

namespace CalorieTracker.Mappings
{
    public class AppRoleMappingProfile : Profile, IMappingProfile
    {
        public AppRoleMappingProfile()
        {
            CreateMap<AppRole, RoleViewModel>();
        }
    }
}