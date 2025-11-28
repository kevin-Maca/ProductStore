using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.DTOs;
using ProductStore.Web.Models;
using ProductStore.Web.Services.Abstractions;
using ProductStore.Web.Services.Implementations;
using System.Diagnostics;

namespace ProductStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeServices _homeServices;

        public HomeController(IHomeServices homeServices)
        {
            _homeServices = homeServices;
        }

            [Authorize]
            [HttpGet]
            public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
            {
                Response<PaginationResponse<CategoryDTO>> response = await _homeServices.GetCategoryAsync(request);
                return View(response.Result);
            }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Category([FromRoute] Guid id, [FromQuery] PaginationRequest request)
        {
            Response<CategoryDTO> response = await _homeServices.GetCategoryAsync(id, request);
            return View(response.Result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Product([FromRoute] Guid id)
        {
            Response<ProductDTO> response = await _homeServices.GetProductAsync(id);
            return View(response.Result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
