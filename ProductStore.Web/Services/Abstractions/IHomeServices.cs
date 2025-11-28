using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.DTOs;

namespace ProductStore.Web.Services.Abstractions
{
    public interface IHomeServices
    {
        public Task<Response<ProductDTO>> GetProductAsync(Guid id);
        public Task<Response<CategoryDTO>> GetCategoryAsync(Guid id, PaginationRequest request);
        public Task<Response<PaginationResponse<CategoryDTO>>> GetCategoryAsync(PaginationRequest request);
    }
}
