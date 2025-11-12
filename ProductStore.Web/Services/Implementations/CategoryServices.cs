using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductStore.Web.Core;
using ProductStore.Web.Data;
using ProductStore.Web.Data.Entities;
using ProductStore.Web.DTOs;
using ProductStore.Web.Services.Abstractions;

namespace ProductStore.Web.Services.Implementations
{
    public class CategoryServices : ICategoryServices
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoryServices(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<CategoryDTO>> CreateAsync(CategoryDTO dto)
        {
            try
            {
                Category category = _mapper.Map<Category>(dto);

                Guid id = Guid.NewGuid();
                category.Id = id;
                await _context.Category.AddAsync(category);
                await _context.SaveChangesAsync();

                dto.Id = id;
                return Response<CategoryDTO>.Success(dto, "Categoría creada con éxito");
            }
            catch (Exception ex)
            {
                return Response<CategoryDTO>.Failure(ex);
            }
        }

        public async Task<Response<List<CategoryDTO>>> GetListAsync()
        {
            try
            {
                List<Category> sections = await _context.Category.ToListAsync();

                List<CategoryDTO> list = _mapper.Map<List<CategoryDTO>>(sections);

                return Response<List<CategoryDTO>>.Success(list);
            }
            catch (Exception ex)
            {
                return Response<List<CategoryDTO>>.Failure(ex);
            }
        }
    }
}
