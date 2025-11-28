using AspNetCoreHero.ToastNotification.Abstractions;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Attributes;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.DTOs;
using ProductStore.Web.Services.Abstractions;
using System.Runtime.CompilerServices;

namespace PrivateBlog.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesServices _rolesService;
        private readonly INotyfService _notyfService;

        public RolesController(IRolesServices rolesService, INotyfService notyfService)
        {
            _rolesService = rolesService;
            _notyfService = notyfService;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showRoles", module: "Roles")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<ProductStoreRoleDTO>> response = await _rolesService.GetPaginatedListAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create()
        {
            Response<List<PermissionsForRoleDTO>> permissionsResponse = await _rolesService.GetPermissionsAsync();
            if (!permissionsResponse.IsSuccess)
            {
                _notyfService.Error(permissionsResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            Response<List<CategoriesForRoleDTO>> categoriesResponse = await _rolesService.GetCategoryAsync();
            if (!categoriesResponse.IsSuccess)
            {
                _notyfService.Error(categoriesResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            ProductStoreRoleDTO dto = new ProductStoreRoleDTO
            {
                Permissions = permissionsResponse.Result,
                Categories = categoriesResponse.Result
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create(ProductStoreRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");

                Response<List<PermissionsForRoleDTO>> permissionsResponse = await _rolesService.GetPermissionsAsync();
                if (!permissionsResponse.IsSuccess)
                {
                    _notyfService.Error(permissionsResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                Response<List<CategoriesForRoleDTO>> categoriesResponse = await _rolesService.GetCategoryAsync();
                if (!categoriesResponse.IsSuccess)
                {
                    _notyfService.Error(categoriesResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                dto.Permissions = permissionsResponse.Result;
                dto.Categories = categoriesResponse.Result;

                return View(dto);
            }

            Response<ProductStoreRoleDTO> createResponse = await _rolesService.CreateAsync(dto);
            if (createResponse.IsSuccess)
            {
                _notyfService.Success(createResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(createResponse.Message);

            Response<List<PermissionsForRoleDTO>> permissionsResponse2 = await _rolesService.GetPermissionsAsync();
            if (!permissionsResponse2.IsSuccess)
            {
                _notyfService.Error(permissionsResponse2.Message);
                return RedirectToAction(nameof(Index));
            }

            Response<List<CategoriesForRoleDTO>> categoriesResponse2 = await _rolesService.GetCategoryAsync();
            if (!categoriesResponse2.IsSuccess)
            {
                _notyfService.Error(categoriesResponse2.Message);
                return RedirectToAction(nameof(Index));
            }

            dto.Permissions = permissionsResponse2.Result;
            dto.Categories = categoriesResponse2.Result;
            return View(dto);
        }


        [HttpGet]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(Guid id)
        {
            Response<ProductStoreRoleDTO> response = await _rolesService.GetOneAsync(id);
            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(ProductStoreRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");

                Response<List<PermissionsForRoleDTO>> permissionsResponse = await _rolesService.GetPermissionsAsync();
                if (!permissionsResponse.IsSuccess)
                {
                    _notyfService.Error(permissionsResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                Response<List<CategoriesForRoleDTO>> categoriesResponse = await _rolesService.GetCategoryAsync();
                if (!categoriesResponse.IsSuccess)
                {
                    _notyfService.Error(categoriesResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                dto.Permissions = permissionsResponse.Result;
                dto.Categories = categoriesResponse.Result;

                return View(dto);
            }

            Response<ProductStoreRoleDTO> updateResponse = await _rolesService.EditAsync(dto);
            if (updateResponse.IsSuccess)
            {
                _notyfService.Success(updateResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(updateResponse.Message);

            Response<List<PermissionsForRoleDTO>> permissionsResponse2 = await _rolesService.GetPermissionsAsync();

            if (!permissionsResponse2.IsSuccess)
            {
                _notyfService.Error(permissionsResponse2.Message);
                return RedirectToAction(nameof(Index));
            }

            Response<List<CategoriesForRoleDTO>> categoriesResponse2 = await _rolesService.GetCategoryAsync();
            if (!categoriesResponse2.IsSuccess)
            {
                _notyfService.Error(categoriesResponse2.Message);
                return RedirectToAction(nameof(Index));
            }

            dto.Permissions = permissionsResponse2.Result;
            dto.Categories = categoriesResponse2.Result;
            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize("deleteRoles", "Roles")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response<object> response = await _rolesService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
            }
            else
            {
                _notyfService.Success(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
