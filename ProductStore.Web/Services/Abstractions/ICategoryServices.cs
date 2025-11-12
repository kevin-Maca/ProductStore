using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.DTOs;

namespace ProductStore.Web.Services.Abstractions
{
    public interface ICategoryServices
    {
        public Task<Response<CategoryDTO>> CreateAsync(CategoryDTO dto);
        public Task<Response<object>> DeleteAsync(Guid id);
        public Task<Response<CategoryDTO>> EditAsync(CategoryDTO dto);
        public Task<Response<List<CategoryDTO>>> GetListAsync();
        public Task<Response<CategoryDTO>> GetOneAsync(Guid id);
        public Task<Response<PaginationResponse<CategoryDTO>>> GetPaginatedListAsync(PaginationRequest request);
    }
}
