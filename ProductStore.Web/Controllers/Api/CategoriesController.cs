using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Attributes;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.DTOs;
using ProductStore.Web.Services.Abstractions;

namespace ProductStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : ApiController
    {
        private readonly ICategoryServices _categoryService;

        public CategoriesController(ICategoryServices categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ApiAuthorize(permission: "showCategories", module: "Categorías")]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<CategoryDTO>> response = await _categoryService.GetPaginatedListAsync(request);
            return ControllerBasicValidation(response);
        }

        [HttpGet("{id}")]
        [ApiAuthorize(permission: "showCategories", module: "Categorías")]
        public async Task<IActionResult> GetOne([FromRoute] Guid id)
        {
            Response<CategoryDTO> response = await _categoryService.GetOneAsync(id);
            return ControllerBasicValidation(response);
        }

        [HttpPost]
        [ApiAuthorize(permission: "showCategories", module: "Categorías")]
        public async Task<IActionResult> Create([FromBody] CategoryDTO dto)
        {
            Response<CategoryDTO> response = await _categoryService.CreateAsync(dto);
            return ControllerBasicValidation(response, ModelState);
        }

        [HttpPut]
        [ApiAuthorize(permission: "updateCategories", module: "Categorías")]
        public async Task<IActionResult> Edit([FromBody] CategoryDTO dto)
        {
            Response<CategoryDTO> response = await _categoryService.EditAsync(dto);
            return ControllerBasicValidation(response, ModelState);
        }

        [HttpDelete("{id}")]
        [ApiAuthorize(permission: "deleteCategories", module: "Categorías")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response<object> response = await _categoryService.DeleteAsync(id);
            return ControllerBasicValidation(response);
        }
    }
}
