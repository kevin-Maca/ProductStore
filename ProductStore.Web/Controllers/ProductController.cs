using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Attributes;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.DTOs;
using ProductStore.Web.Helpers.Abstractions;
using ProductStore.Web.Services.Abstractions;

namespace ProductStore.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IProductServices _productService;
        private readonly ICombosHelper _combosHelper;

        public ProductController(INotyfService notyfService, IProductServices productService, ICombosHelper combosHelper)
        {
            _notyfService = notyfService;
            _productService = productService;
            _combosHelper = combosHelper;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showProducts", module: "Product")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<ProductDTO>> response = await _productService.GetPaginatedListAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createProducts", module: "Product")]
        public async Task<IActionResult> Create()
        {
            ProductDTO dto = new ProductDTO
            {
                Categories = await _combosHelper.GetComboCategory()
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createProducts", module: "Product")]
        public async Task<IActionResult> Create(ProductDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                dto.Categories = await _combosHelper.GetComboCategory();
                return View(dto);
            }

            Response<ProductDTO> response = await _productService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                dto.Categories = await _combosHelper.GetComboCategory();
                return View(dto);
            }

            _notyfService.Success(response.Message);
            dto.Categories = await _combosHelper.GetComboCategory();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [CustomAuthorize(permission: "updateProduct", module: "Product")]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            Response<ProductDTO> response = await _productService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            response.Result.Categories = await _combosHelper.GetComboCategory();
            return View(response.Result);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateProduct", module: "Product")]
        public async Task<IActionResult> Edit(ProductDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                dto.Categories = await _combosHelper.GetComboCategory();
                return View(dto);
            }

            Response<ProductDTO> response = await _productService.EditAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                dto.Categories = await _combosHelper.GetComboCategory();
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [CustomAuthorize(permission: "deleteProduct", module: "Product")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response<object> response = await _productService.DeleteAsync(id);

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
