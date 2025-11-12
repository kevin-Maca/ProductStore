using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.DTOs;

namespace ProductStore.Web.Services.Abstractions
{
    public interface IProductServices
    {
        public Task<Response<ProductDTO>> CreateAsync(ProductDTO dto);
        public Task<Response<object>> DeleteAsync(Guid id);
        public Task<Response<ProductDTO>> EditAsync(ProductDTO dto);
        public Task<Response<ProductDTO>> GetOneAsync(Guid id);
        public Task<Response<PaginationResponse<ProductDTO>>> GetPaginatedListAsync(PaginationRequest request);
    }
}
