using AutoMapper;
using CalorieTracker.Models;
using CalorieTracker.Models.Requests;
using CalorieTracker.Models.ViewModels;

namespace CalorieTracker.Mappings
{
    public class AppUserMappingProfile : Profile, IMappingProfile
    {
        public AppUserMappingProfile()
        {
            CreateMap<RegisterUserRequest, AppUser>();
            CreateMap<AppUser, UserViewModel>();
            CreateMap<UserInformationRequest, UserInformation>();
        }
    }
}