using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Web.Core;
using ProductStore.Web.Data.Entities;
using ProductStore.Web.DTOs;
using ProductStore.Web.Services.Abstractions;
using ProductStore.Web.Services.Implementations;

namespace ProductStore.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersServices _usersServices;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyfService;

        public AccountController(IUsersServices usersServices, IMapper mapper, INotyfService notyfService)
        {
            _usersServices = usersServices;
            _mapper = mapper;
            this._notyfService = notyfService;
        }

        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (ModelState.IsValid)
            {
                Response<Microsoft.AspNetCore.Identity.SignInResult> result = await _usersServices.LoginAsync(dto);

                if (result.IsSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
            }

            return View(dto);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _usersServices.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateUser()
        {
            User user = await _usersServices.GetUserByEmailAsync(User.Identity.Name);

            if (user is null)
            {
                return NotFound();
            }

            return View(_mapper.Map<AccountUserDTO>(user));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(AccountUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                Response<AccountUserDTO> result = await _usersServices.UpdateUserAsync(dto);

                if (result.IsSuccess)
                {
                    _notyfService.Success(result.Message);
                }
                else
                {
                    _notyfService.Error(result.Message);
                }

                return RedirectToAction("Index", "Home");
            }

            _notyfService.Error("Debe ajustar lo errores de validación");
            return View(dto);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar lo errores de validación");
                return View();
            }

            User user = await _usersServices.GetUserByEmailAsync(User.Identity.Name);

            bool isCorrectPassword = await _usersServices.CheckPasswordAsync(user, dto.CurrentPassword);

            if (!isCorrectPassword)
            {
                _notyfService.Error("La contraseña actual es incorrecta");
                return View();
            }

            string resetToken = await _usersServices.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _usersServices.ResetPasswordAsync(user, resetToken, dto.NewPassword);

            if (!result.Succeeded)
            {
                _notyfService.Error("Ha ocurrido un error al intentar actualizar su contraseña");
                return View(dto);
            }

            _notyfService.Success("Contraseña actualizada con éxito");
            return RedirectToAction("Index", "Home");
        }
    }
}
