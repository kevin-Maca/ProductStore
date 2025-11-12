using AutoMapper;
using ProductStore.Web.DTOs;
using static System.Collections.Specialized.BitVector32;

namespace ProductStore.Web.Core
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Section, CategoryDTO>().ReverseMap();
        }
    }
}
