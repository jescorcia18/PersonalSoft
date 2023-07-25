using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SeguroAPI.Dtos.Login;
using SeguroAPI.Dtos.Role;
using SeguroAPI.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace SeguroAPI.Controllers
{
    [ApiController]
    [Route("api/v1/authenticate")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("roles/add")]
        [Authorize]
        public async Task<IActionResult> CreateRol([FromBody] CreateRoleRequest request)
        {
            var appRole = new ApplicationRole { Name = request.Role };
            var createRol = await _roleManager.CreateAsync(appRole);
            return Ok(new { message = "Rol creado exitosamente." });
        }

        [HttpPost]
        [Route("register")]
        [Authorize]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await RegisterAsync(request);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        private async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(request.Email);
                if (userExists != null)
                    return new RegisterResponse { Message = "Usuario ya Existe.", Success = false };

                userExists = new ApplicationUser
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    UserName = request.UserName
                };

                var createUserResult = await _userManager.CreateAsync(userExists, request.Password);
                if (!createUserResult.Succeeded)
                    return new RegisterResponse { Message = $"Creacion de Usuario Falló {createUserResult?.Errors?.First()?.Description}", Success = false };

                //Validacion de Role
                var appRole = new ApplicationRole { Name = request.Role };
                var existsRole = await _roleManager.GetRoleNameAsync(appRole);
                if (existsRole is null)
                    return new RegisterResponse { Message = $"Creacion de Usuario Falló {createUserResult?.Errors?.First()?.Description}, Rol no Existe.", Success = false };

                // usuario es Creado - No Se agrega un rol al Usuario
                var addUserToRolResult = await _userManager.AddToRoleAsync(userExists, request.Role);
                if (!addUserToRolResult.Succeeded)
                    return new RegisterResponse { Message = $"Usuario ha Sido Creado, pero no se le puede agregar un Rol. {addUserToRolResult?.Errors?.First()?.Description}", Success = false };

                // Usuario creado y con un Rol asignado.

                return new RegisterResponse
                {
                    Success = true,
                    Message = "Usuario registrado exitosamente."
                };
            }
            catch (Exception ex)
            {
                return new RegisterResponse
                {
                    Message = ex.Message,
                    Success = false
                };
            }

        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LoginResponse))]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await LoginAsync(request);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        private async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                    return new LoginResponse { Message = "Invalido Email / Password", Success = false };

                var claims = new List<Claim>
                {
                    new Claim (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim (ClaimTypes.Name, user.UserName),
                    new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                };
                var roles = await _userManager.GetRolesAsync(user);
                var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
                claims.AddRange(roleClaims);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1swek3u4uo2u4a6e"));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddMinutes(30);

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: expires,
                    signingCredentials: credentials
                    );

                return new LoginResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Message = "Login Exitoso!",
                    Success = true,
                    Email = user?.Email,
                    UserId = user?.Id.ToString()


                };
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return new LoginResponse
                {
                    Message = ex.Message,
                    Success = false
                };
            }

        }
    }
}
