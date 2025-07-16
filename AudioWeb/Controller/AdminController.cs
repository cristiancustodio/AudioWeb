using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AudioWeb.Models; // Para ApplicationUser e ApplicationRole
using AudioWeb.Shared.Models; // Para os DTOs

namespace WebAudio.Controllers
{
    [Authorize(Roles = "Administrador")] // Apenas usuários com o papel "Administrador" podem acessar
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    NomeCompleto = user.NomeCompleto,
                    Roles = roles.ToList()
                });
            }
            return Ok(userDtos);
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var roles = await _roleManager.Roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name }).ToListAsync();
            return Ok(roles);
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = request.Email, // Ou um nome de usuário único
                Email = request.Email,
                NomeCompleto = request.NomeCompleto
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Atribuir o papel selecionado
            if (!string.IsNullOrEmpty(request.SelectedRole))
            {
                var roleExists = await _roleManager.RoleExistsAsync(request.SelectedRole);
                if (!roleExists)
                {
                    // Opcional: criar o papel se não existir, ou retornar erro
                    await _roleManager.CreateAsync(new ApplicationRole { Name = request.SelectedRole });
                }
                await _userManager.AddToRoleAsync(user, request.SelectedRole);
            }

            return Ok();
        }

        [HttpPost("update-user-roles")]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Adicionar papéis
            if (request.RolesToAdd.Any())
            {
                var result = await _userManager.AddToRolesAsync(user, request.RolesToAdd);
                if (!result.Succeeded) return BadRequest(result.Errors);
            }

            // Remover papéis
            if (request.RolesToRemove.Any())
            {
                var result = await _userManager.RemoveFromRolesAsync(user, request.RolesToRemove);
                if (!result.Succeeded) return BadRequest(result.Errors);
            }

            return Ok();
        }

        // Você também precisaria de endpoints para:
        // - Excluir usuário
        // - Editar detalhes do usuário (não relacionados a papéis)
        // - Gerenciar papéis (criar, excluir, editar papéis em si)
    }
}