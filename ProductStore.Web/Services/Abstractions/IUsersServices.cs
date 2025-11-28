using Microsoft.AspNetCore.Identity;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.Data.Entities;
using ProductStore.Web.DTOs;

namespace ProductStore.Web.Services.Abstractions
{
    public interface IUsersServices
    {
        public Task<Response<IdentityResult>> AddUserAsync(User user, string password);
        public Task<bool> CheckPasswordAsync(User user, string password);
        public Task<Response<IdentityResult>> ConfirmUserAsync(User user, string token);
        public bool CurrentUserIsAuthenticaded();
        public Task<bool> CurrentUserIsSuperAdminAsync();
        public Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module);
        public Task<Response<string>> GenerateConfirmationTokenAsync(User user);
        public Task<string> GeneratePasswordResetTokenAsync(User user);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<User> GetUserByIdAsync(Guid id);
        public Task<Response<SignInResult>> LoginAsync(LoginDTO dto);
        public Task<Response<UserTokenDTO>> LoginApiAsync(LoginDTO dto);
        public Task LogoutAsync();
        public Task<IdentityResult> ResetPasswordAsync(User user, string resetToken, string newPassword);
        public Task<Response<AccountUserDTO>> UpdateUserAsync(AccountUserDTO dto);


        // For Management
        public Task<Response<UserDTO>> CreateAsync(UserDTO dto);
        //public Task<Response<object>> DeleteAsync(Guid id);
        public Task<Response<UserDTO>> EditAsync(UserDTO dto);



        //public Task<Response<UserDTO>> GetOneAsync(Guid id);
        public Task<Response<PaginationResponse<UserDTO>>> GetPaginatedListAsync(PaginationRequest request);
    }
}
