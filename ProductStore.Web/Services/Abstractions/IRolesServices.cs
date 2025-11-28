using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.DTOs;

namespace ProductStore.Web.Services.Abstractions
{
    public interface IRolesServices
    {
        public Task<Response<ProductStoreRoleDTO>> CreateAsync(ProductStoreRoleDTO dto);
        public Task<Response<object>> DeleteAsync(Guid id);
        public Task<Response<ProductStoreRoleDTO>> EditAsync(ProductStoreRoleDTO dto);
        public Task<Response<ProductStoreRoleDTO>> GetOneAsync(Guid id);
        public Task<Response<PaginationResponse<ProductStoreRoleDTO>>> GetPaginatedListAsync(PaginationRequest request);
        public Task<Response<List<PermissionsForRoleDTO>>> GetPermissionsAsync();
        public Task<Response<List<CategoriesForRoleDTO>>> GetCategoryAsync();
    }
}
