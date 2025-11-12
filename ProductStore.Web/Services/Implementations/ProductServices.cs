using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.Data;
using ProductStore.Web.Data.Entities;
using ProductStore.Web.DTOs;
using ProductStore.Web.Services.Abstractions;

namespace ProductStore.Web.Services.Implementations
{
    public class ProductServices : CustomQueryableOperationsService, IProductServices
    {
        private readonly DataContext _context;
        private readonly DataContext _mapper;

        public ProductServices(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }

        public async Task<Response<ProductDTO>> CreateAsync(ProductDTO dto)
        {
            return await CreateAsync<Product, ProductDTO>(dto);
        }

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            return await DeleteAsync<Product>(id);
        }

        public async Task<Response<ProductDTO>> EditAsync(ProductDTO dto)
        {
            return await EditAsync<Product, ProductDTO>(dto, dto.Id);
        }

        public async Task<Response<ProductDTO>> GetOneAsync(Guid id)
        {
            return await GetOneAsync<Product, ProductDTO>(id);
        }

        public async Task<Response<PaginationResponse<ProductDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<Product> query = _context.Product.Include(p => p.Category)
                                                   .Select(p => new Product
                                                   {
                                                       Id = p.Id,
                                                       Name = p.Name,

                                                       Category = new Category
                                                       {
                                                           Id = p.Category.Id,
                                                           Name = p.Category.Name
                                                       },

                                                       categoryId = p.categoryId
                                                   })
                                                   .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<Product, ProductDTO>(request, query);

        }
    }
}
