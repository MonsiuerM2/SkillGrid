using AutoMapper;
using DMed_Razor.DTOs.AMEs;
using DMed_Razor.DTOs.CMEs;
using DMed_Razor.Entities;

namespace DMed_Razor.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<RegisterDto, AppUser>();
            CreateMap<ModulePreReqs, ModulePreReqsDto>();
        }
    }
}