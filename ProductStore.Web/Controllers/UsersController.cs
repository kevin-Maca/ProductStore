using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Attributes;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.Data.Entities;
using ProductStore.Web.DTOs;
using ProductStore.Web.Helpers.Abstractions;
using ProductStore.Web.Services.Abstractions;
using ProductStore.Web.Services.Implementations;

namespace ProductStore.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersServices _usersServices;
        private readonly INotyfService _notifyService;
        private readonly ICombosHelper _combosHelper;
        private readonly IMapper _mapper;

        public UsersController(IUsersServices usersServices, INotyfService notifyService, ICombosHelper combosHelper, IMapper mapper)
        {
            _usersServices = usersServices;
            _notifyService = notifyService;
            _combosHelper = combosHelper;
            _mapper = mapper;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showUsers", module: "Usuarios")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<UserDTO>> response = await _usersServices.GetPaginatedListAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createUsers", module: "Usuarios")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<SelectListItem> items = await _combosHelper.GetComboRoles();

            UserDTO dto = new UserDTO
            {
                ProductStoreRoles = items,
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createUsers", module: "Usuarios")]
        public async Task<IActionResult> Create(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                dto.ProductStoreRoles = await _combosHelper.GetComboRoles();
                return View(dto);
            }

            Response<UserDTO> response = await _usersServices.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                dto.ProductStoreRoles = await _combosHelper.GetComboRoles();
                return View(dto);
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        [CustomAuthorize(permission: "updateUsers", module: "Usuarios")]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (Guid.Empty.Equals(id))
            {
                return NotFound();
            }

            User user = await _usersServices.GetUserByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            UserDTO dto = _mapper.Map<UserDTO>(user);
            dto.ProductStoreRoles = await _combosHelper.GetComboRoles();

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateUsers", module: "Usuarios")]
        public async Task<IActionResult> Edit(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                dto.ProductStoreRoles = await _combosHelper.GetComboRoles();
                return View(dto);
            }

            Response<UserDTO> response = await _usersServices.EditAsync(dto);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                dto.ProductStoreRoles = await _combosHelper.GetComboRoles();
                return View(dto);
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
