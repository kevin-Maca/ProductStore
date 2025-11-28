using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.Data;
using ProductStore.Web.Data.Entities;
using ProductStore.Web.DTOs;
using ProductStore.Web.Services.Abstractions;
using System.Reflection.Metadata;
using static System.Collections.Specialized.BitVector32;
using ClaimsUser = System.Security.Claims.ClaimsPrincipal;

namespace ProductStore.Web.Services.Implementations
{
    public class HomeServices : CustomQueryableOperationsService, IHomeServices
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersServices _usersService;
        private readonly IMapper _mapper;

        public HomeServices(DataContext context,
                           IHttpContextAccessor httpContextAccessor,
                           IUsersServices usersService,
                           IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _usersService = usersService;
            _mapper = mapper;
        }


        public async Task<Response<PaginationResponse<CategoryDTO>>> GetCategoryAsync(PaginationRequest request)
        {
            ClaimsUser? claimsUser = _httpContextAccessor.HttpContext?.User;
            string? userName = claimsUser.Identity.Name;
            User user = await _usersService.GetUserByEmailAsync(userName);

            IQueryable<Category> query = _context.Category.Include(c => c.RoleCategories);

            if (!await _usersService.CurrentUserIsSuperAdminAsync())
            {
                query = query.Where(c => c.RoleCategories.Any(rc => rc.ProductStoreRoleId == user.ProductStoreRoleId));
            }


            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(c => c.Name.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<Category, CategoryDTO>(request, query);
        }

        public async Task<Response<ProductDTO>> GetProductAsync(Guid id)
        {
            return await GetOneAsync<Product, ProductDTO>(id);
        }

        public async Task<Response<CategoryDTO>> GetCategoryAsync(Guid id, PaginationRequest request)
        {
            try
            {
                Category? category = await _context.Category.Include(s => s.RoleCategories)
                                                          .Where(s =>  s.Id == id)
                                                          .FirstOrDefaultAsync();

                if (category is null)
                {
                    return Response<CategoryDTO>.Failure($"La categoría con id '{id}' no existe.");
                }

                ClaimsUser? claimsUser = _httpContextAccessor.HttpContext?.User;
                string? userName = claimsUser.Identity.Name;
                User user = await _usersService.GetUserByEmailAsync(userName);

                bool isAuthorized = true;
                if (!await _usersService.CurrentUserIsSuperAdminAsync())
                {
                    isAuthorized = category.RoleCategories.Any(rc => rc.ProductStoreRoleId == user.ProductStoreRoleId);
                }

                if (!isAuthorized)
                {
                    return Response<CategoryDTO>.Failure("No tiene autorización para consultar esta categoría");
                }

                IQueryable<Product> query = _context.Product.Where(b => b.CategoryId == category.Id);
                query = query.Select(b => new Product
                {
                    Id = b.Id,
                    Name = b.Name,
                    CategoryId = b.CategoryId
                });

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(b => b.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                Response<PaginationResponse<ProductDTO>> paginationResponse = await GetPaginationAsync<Product, ProductDTO>(request, query);
                if (!paginationResponse.IsSuccess)
                {
                    return Response<CategoryDTO>.Failure(paginationResponse.Message);
                }

                CategoryDTO dto = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    PaginatedProducts = paginationResponse.Result
                };

                return Response<CategoryDTO>.Success(dto);
            }
            catch (Exception ex)
            {
                return Response<CategoryDTO>.Failure(ex);
            }
        }
    }
}
