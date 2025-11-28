using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductStore.Web.Core;
using ProductStore.Web.Core.Pagination;
using ProductStore.Web.Data;
using ProductStore.Web.Data.Entities;
using ProductStore.Web.DTOs;
using ProductStore.Web.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductStore.Web.Services.Implementations
{
    // TODO: Mejorar mensajes de error
    public class UsersServices : CustomQueryableOperationsService, IUsersServices
    {
        private readonly DataContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UsersServices(DataContext context,
                             SignInManager<User> signInManager,
                              UserManager<User> userManager,
                             IHttpContextAccessor httpContextAccessor,
                             IMapper mapper,
                            IConfiguration configuration) : base(context, mapper)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<Response<IdentityResult>> AddUserAsync(User user, string password)
        {
            IdentityResult result = await _userManager.CreateAsync(user, password);

            return new Response<IdentityResult>
            {
                Result = result,
                IsSuccess = result.Succeeded,
            };
        }

        public async Task<Response<IdentityResult>> ConfirmUserAsync(User user, string token)
        {
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

            return new Response<IdentityResult>
            {
                Result = result,
                IsSuccess = result.Succeeded,
            };
        }

        public async Task<Response<UserDTO>> CreateAsync(UserDTO dto)
        {
            try
            {
                User user = _mapper.Map<User>(dto);
                user.Id = Guid.NewGuid().ToString();

                Response<IdentityResult> response = await AddUserAsync(user, dto.Document);

                // TODO: Envío de email para confirmación
                Response<string> token = await GenerateConfirmationTokenAsync(user);

                await ConfirmUserAsync(user, token.Result);

                return Response<UserDTO>.Success(_mapper.Map<UserDTO>(user), "Usuario creado con éxito");
            }
            catch (Exception ex)
            {
                return Response<UserDTO>.Failure(ex);
            }
        }

        public bool CurrentUserIsAuthenticaded()
        {
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            return user?.Identity is not null && user.Identity.IsAuthenticated;
        }

        public async Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module)
        {
            ClaimsPrincipal? claimsUser = _httpContextAccessor.HttpContext?.User;

            // Valida si hay sesión
            if (claimsUser is null)
            {
                return false;
            }

            string userName = claimsUser.Identity!.Name!;

            User? user = await GetUserByEmailAsync(userName);

            if (user is null)
            {
                return false;
            }

            if (user.ProductStoreRole.Name == Env.SUPER_ADMIN_ROLE_NAME)
            {
                return true;
            }

            return await _context.Permission.Include(p => p.RolePermissions)
                                             .AnyAsync(p => (p.Module == module && p.Name == permission)
                                                            && p.RolePermissions.Any(rp => rp.ProductStoreRoleId == user.ProductStoreRoleId));
        }

        public async Task<bool> CurrentUserIsSuperAdminAsync()
        {
            ClaimsPrincipal? claimsUser = _httpContextAccessor.HttpContext?.User;

            // Valida si hay sesión
            if (claimsUser is null)
            {
                return false;
            }

            string userName = claimsUser.Identity!.Name!;

            User? user = await GetUserByEmailAsync(userName);

            if (user is null)
            {
                return false;
            }

            return user.ProductStoreRole.Name == Env.SUPER_ADMIN_ROLE_NAME;
        }

        public async Task<Response<UserDTO>> EditAsync(UserDTO dto)
        {
            try
            {
                User? user = await GetUserAsync(dto.Id.ToString());

                if (user == null)
                {
                    return Response<UserDTO>.Failure("No existe usuario.");
                }

                user.PhoneNumber = dto.PhoneNumber;
                user.Document = dto.Document;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.ProductStoreRoleId = Guid.Parse(dto.ProductStoreRoleId);

                _context.Users.Update(user);

                await _context.SaveChangesAsync();

                return Response<UserDTO>.Success(dto, "Usuario actualizado con éxito");
            }
            catch (Exception ex)
            {
                return Response<UserDTO>.Failure(ex);
            }
        }

        public async Task<Response<string>> GenerateConfirmationTokenAsync(User user)
        {
            string result = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Response<string>.Success(result);
        }

        public async Task<Response<PaginationResponse<UserDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<User> queryable = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
            {
                queryable = queryable.Where(u => u.FirstName.ToLower().Contains(request.Filter.ToLower())
                                                || u.LastName.ToLower().Contains(request.Filter.ToLower())
                                                || u.Document.ToLower().Contains(request.Filter.ToLower())
                                                || u.Email.ToLower().Contains(request.Filter.ToLower())
                                                || u.PhoneNumber.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<User, UserDTO>(request, queryable);
        }


        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.ProductStoreRole)
                                       .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.Include(u => u.ProductStoreRole)
                                       .FirstOrDefaultAsync(u => u.Id == id.ToString());
        }

        public async Task<Response<SignInResult>> LoginAsync(LoginDTO dto)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

            return new Response<SignInResult>
            {
                Result = result,
                IsSuccess = result.Succeeded,
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Response<AccountUserDTO>> UpdateUserAsync(AccountUserDTO dto)
        {
            try
            {
                User user = await GetUserAsync(dto.Id);
                user.PhoneNumber = dto.PhoneNumber;
                user.Document = dto.Document;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;

                _context.Users.Update(user);

                await _context.SaveChangesAsync();

                return Response<AccountUserDTO>.Success(dto, "Datos actualizados con éxito");
            }
            catch (Exception ex)
            {
                return Response<AccountUserDTO>.Failure(ex);
            }
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        private async Task<User> GetUserAsync(string? id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<Response<UserTokenDTO>>LoginApiAsync(LoginDTO dto)
        {
            try
            {

                SignInResult result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

                if (result.Succeeded)
                {
                    User user = await _userManager.FindByEmailAsync(dto.Email);
                    UserTokenDTO token = await BuildTokenAsync(dto.Email, user.Id);

                    return Response<UserTokenDTO>.Success(token, "Token obtenido con éxito");
                }

                return Response<UserTokenDTO>.Failure("Usuario o contraseña incorrectos");
            }
            catch (Exception ex)
            {
                return Response<UserTokenDTO>.Failure(ex);
            }
        }

        private async Task<UserTokenDTO> BuildTokenAsync(string email, string id)
        {
            List<Claim> claims = [

                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim("userId", id)
            ];

            User identityUser = await _userManager.FindByEmailAsync(email);

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime expiration = DateTime.UtcNow.AddYears(100);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new UserTokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        public async Task<IdentityResult>ResetPasswordAsync(User user, string resetToken, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        }
    }
}
