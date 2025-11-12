using ProductStore.Web.Core;
using ProductStore.Web.DTOs;

namespace ProductStore.Web.Services.Abstractions
{
    public interface ICategoryServices
    {
        public Task<Response<CategoryDTO>> CreateAsync(CategoryDTO dto);
        public Task<Response<List<CategoryDTO>>> GetListAsync();
    }
}
