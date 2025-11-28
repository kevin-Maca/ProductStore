using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.Data;
using ProductStore.Web.Data.Entities;
using ProductStore.Web.DTOs;
using ProductStore.Web.Services.Abstractions;
using static System.Collections.Specialized.BitVector32;

namespace ProductStore.Web.Services.Implementations
{
    public class RolesServices : CustomQueryableOperationsService, IRolesServices
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RolesServices(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<ProductStoreRoleDTO>> CreateAsync(ProductStoreRoleDTO dto)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Guid newRoleId = Guid.NewGuid();

                    // Role
                    ProductStoreRole role = _mapper.Map<ProductStoreRole>(dto);

                    await _context.ProductStoreRole.AddAsync(role);

                    await _context.SaveChangesAsync();

                    // Permissions
                    List<Guid> permissionIds = new();

                    if (!string.IsNullOrEmpty(dto.PermissionIds))
                    {
                        permissionIds = JsonConvert.DeserializeObject<List<Guid>>(dto.PermissionIds);
                    }

                    foreach (Guid permissionId in permissionIds)
                    {
                        RolePermission rolePermission = new RolePermission
                        {
                            ProductStoreRoleId = role.Id,
                            PermissionId = permissionId
                        };

                        await _context.RolePermission.AddAsync(rolePermission);
                    }

                    List<Guid> categoryIds = new();

                    if (!string.IsNullOrEmpty(dto.CategoryIds))
                    {
                        categoryIds = JsonConvert.DeserializeObject<List<Guid>>(dto.CategoryIds);
                    }

                    foreach (Guid categoryId in categoryIds)
                    {
                        RoleCategory roleCategory = new RoleCategory
                        {
                            ProductStoreRoleId = role.Id,
                            CategoryId = categoryId
                        };

                        await _context.RoleCategories.AddAsync(roleCategory);
                    }


                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return Response<ProductStoreRoleDTO>.Success(dto, "Rol creado con éxito");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Response<ProductStoreRoleDTO>.Failure(ex);
                }
            }
        }

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            if (_context.Users.Any(u => u.ProductStoreRoleId == id))
            {
                return Response<object>.Failure("No puede eliminar el rol ya que existen usuarios que lo contienen");
            }

            return await DeleteAsync<ProductStoreRole>(id);
        }

        public async Task<Response<ProductStoreRoleDTO>> EditAsync(ProductStoreRoleDTO dto)
        {
            try
            {
                if (dto.Name == Env.SUPER_ADMIN_ROLE_NAME)
                {
                    return Response<ProductStoreRoleDTO>.Failure($"El rol '{Env.SUPER_ADMIN_ROLE_NAME}' no puede ser editado");
                }

                // Role
                ProductStoreRole role = _mapper.Map<ProductStoreRole>(dto);
                _context.ProductStoreRole.Update(role);

                // Permissions
                List<Guid> permissionIds = new();

                if (!string.IsNullOrEmpty(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<Guid>>(dto.PermissionIds);
                }

                // Delete old
                List<RolePermission> oldRolePermissions = await _context.RolePermission.Where(rp => rp.ProductStoreRoleId == dto.Id).ToListAsync();
                _context.RolePermission.RemoveRange(oldRolePermissions);

                // Create new ones
                foreach (Guid permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        ProductStoreRoleId = role.Id,
                        PermissionId = permissionId
                    };

                    await _context.RolePermission.AddAsync(rolePermission);
                }

                List<Guid> categoryIds = new();

                if (!string.IsNullOrEmpty(dto.CategoryIds))
                {
                    categoryIds = JsonConvert.DeserializeObject<List<Guid>>(dto.CategoryIds);
                }

                // Delete old
                List<RoleCategory> oldRoleCategories = await _context.RoleCategories.Where(rp => rp.ProductStoreRoleId == dto.Id).ToListAsync();
                _context.RoleCategories.RemoveRange(oldRoleCategories);

                // Create new ones
                foreach (Guid categoryId in categoryIds)
                {
                    RoleCategory roleCategory = new RoleCategory
                    {
                        ProductStoreRoleId = role.Id,
                        CategoryId = categoryId
                    };

                    await _context.RoleCategories.AddAsync(roleCategory);
                }

                await _context.SaveChangesAsync();

                return Response<ProductStoreRoleDTO>.Success(dto, "Rol actualizado con éxito");
            }
            catch (Exception ex)
            {
                return Response<ProductStoreRoleDTO>.Failure(ex);
            }
        }

        public async Task<Response<ProductStoreRoleDTO>> GetOneAsync(Guid id)
        {
            Response<ProductStoreRoleDTO> response = await GetOneAsync<ProductStoreRole, ProductStoreRoleDTO>(id);

            if (!response.IsSuccess)
            {
                return response;
            }

            ProductStoreRoleDTO dto = response.Result;

            List<PermissionsForRoleDTO> permissions = await _context.Permission.Select(p => new PermissionsForRoleDTO
            {
                Id = p.Id,
                Description = p.Description,
                Module = p.Module,
                Selected = _context.RolePermission.Any(rp => rp.PermissionId == p.Id && rp.ProductStoreRoleId == dto.Id)
            }).ToListAsync();

            dto.Permissions = permissions;

            List<CategoriesForRoleDTO> categories = await _context.Category.Select(p => new CategoriesForRoleDTO
            {
                Id = p.Id,
                Name = p.Name,
                Selected = _context.RoleCategories.Any(rc => rc.CategoryId == p.Id && rc.ProductStoreRoleId == dto.Id)
            }).ToListAsync();

            dto.Categories = categories;


            return Response<ProductStoreRoleDTO>.Success(dto, "Rol obtenido con éxito");
        }

        public async Task<Response<PaginationResponse<ProductStoreRoleDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<ProductStoreRole> query = _context.ProductStoreRole.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(r => r.Name.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<ProductStoreRole, ProductStoreRoleDTO>(request, query);
        }

        public async Task<Response<List<PermissionsForRoleDTO>>> GetPermissionsAsync()
        {
            Response<List<PermissionDTO>> permissionsResponse = await GetCompleteListAsync<Permission, PermissionDTO>();

            if (!permissionsResponse.IsSuccess)
            {
                return Response<List<PermissionsForRoleDTO>>.Failure(permissionsResponse.Message);
            }

            List<PermissionsForRoleDTO> dto = permissionsResponse.Result.Select(p => new PermissionsForRoleDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,
                Selected = false
            }).ToList();

            return Response<List<PermissionsForRoleDTO>>.Success(dto);
        }

        public async Task<Response<List<CategoriesForRoleDTO>>> GetCategoryAsync()
        {
            Response<List<CategoryDTO>> categoriesResponse = await GetCompleteListAsync<Category, CategoryDTO>();

            if (!categoriesResponse.IsSuccess)
            {
                return Response<List<CategoriesForRoleDTO>>.Failure(categoriesResponse.Message);
            }

            List<CategoriesForRoleDTO> dto = categoriesResponse.Result.Select(p => new CategoriesForRoleDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Selected = false
            }).ToList();

            return Response<List<CategoriesForRoleDTO>>.Success(dto);
        }
    }
}
