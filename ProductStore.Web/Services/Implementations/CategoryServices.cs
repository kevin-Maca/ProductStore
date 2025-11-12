using AutoMapper;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.Data;
using ProductStore.Web.Data.Entities;
using ProductStore.Web.DTOs;
using ProductStore.Web.Services.Abstractions;

namespace ProductStore.Web.Services.Implementations
{
    public class CategoryServices : CustomQueryableOperationsService, ICategoryServices
    {
        private readonly DataContext _context;
        private readonly DataContext _mapper;

        public CategoryServices(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }

        public async Task<Response<CategoryDTO>> CreateAsync(CategoryDTO dto)
        {
            return await CreateAsync<Category, CategoryDTO>(dto);
        }

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            return await DeleteAsync<Category>(id);
        }

        public async Task<Response<CategoryDTO>> EditAsync(CategoryDTO dto)
        {
            return await EditAsync<Category, CategoryDTO>(dto, dto.Id);
        }

        public async Task<Response<List<CategoryDTO>>> GetListAsync()
        {
            return await GetCompleteListAsync<Category, CategoryDTO>();
        }

        public async Task<Response<CategoryDTO>> GetOneAsync(Guid id)
        {
            return await GetOneAsync<Category, CategoryDTO>(id);
        }
        public async Task<Response<PaginationResponse<CategoryDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<Category> query = _context.Category.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(c => c.Name.ToLower().Contains(request.Filter.ToLower())
                                         || c.Description.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<Category, CategoryDTO>(request, query);
        }
    }
}
