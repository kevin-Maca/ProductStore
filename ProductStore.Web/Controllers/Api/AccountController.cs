using Microsoft.AspNetCore.Mvc;
using ProductStore.Web.DTOs;
using ProductStore.Web.Services.Abstractions;

namespace ProductStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ApiController
    { 
        private readonly IUsersServices _usersService;

        public AccountController(IUsersServices usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginDTO dto)
        {
            return ControllerBasicValidation(await _usersService.LoginApiAsync(dto));
        }
    }
}
