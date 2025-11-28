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
            CreateMap<User, AccountUserDTO>().ReverseMap();
            CreateMap<Permission, PermissionDTO>();
            CreateMap<ProductStoreRole, ProductStoreRoleDTO>().ReverseMap();


            CreateMap<User, UserDTO>();

            CreateMap<UserDTO, User>().ForMember(user => user.UserName, config => config.MapFrom(dto => dto.Email));
        }
    }
}
