using Microsoft.AspNetCore.Identity;
using ProductStore.Web.Core;
using ProductStore.Web.Data.Entities;
using ProductStore.Web.DTOs;

namespace ProductStore.Web.Services.Abstractions
{
    public interface IUsersServices
    {
        public Task<Response<IdentityResult>> AddUserAsync(User user, string password);
        public Task<Response<IdentityResult>> ConfirmUserAsync(User user, string token);
        public Task<Response<string>> GenerateConfirmationTokenAsync(User user);
        public Task<Response<SignInResult>> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();
    }
}
