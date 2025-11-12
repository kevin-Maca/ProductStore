using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.DTOs;
using ProductStore.Web.Services.Abstractions;

namespace ProductStore.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryServices _categoryService;
        private readonly INotyfService _notyfService;

        public CategoryController(ICategoryServices categoryService, INotyfService notyfService)
        {
            _categoryService = categoryService;
            _notyfService = notyfService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<CategoryDTO>> response = await _categoryService.GetPaginatedListAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<CategoryDTO> response = await _categoryService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            Response<CategoryDTO> response = await _categoryService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CategoryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<CategoryDTO> response = await _categoryService.EditAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response<object> response = await _categoryService.DeleteAsync(id);

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
