using AutoMapper;
using ProductStore.Web.Data.Entities;
using ProductStore.Web.DTOs;

namespace ProductStore.Web.Core
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
