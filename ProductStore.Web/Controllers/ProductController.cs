using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.DTOs;
using ProductStore.Web.Helpers.Abstractions;
using ProductStore.Web.Services.Abstractions;

namespace ProductStore.Web.Controllers
{
    //[Authorize]
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
        public async Task<IActionResult> Create()
        {
            ProductDTO dto = new ProductDTO
            {
                Categories = await _combosHelper.GetComboCategory()
            };

            return View(dto);
        }

        [HttpPost]
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
    }
}
