namespace AudioWeb.Shared.Models
{
    public class UserDto
    {
        
        public required string Id { get; set; }

        public required string UserName { get; set; }

        public required string Email { get; set; }

        public required string NomeCompleto { get; set; }

        public List<string> Roles { get; set; } = new List<string>(); // Papéis do usuário
    }

    public class CreateUserRequest
    {

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string NomeCompleto { get; set; }

        public required string SelectedRole { get; set; } // Para o administrador selecionar
    }

    public class UpdateUserRolesRequest
    {
        public required string UserId { get; set; }
        public List<string> RolesToAdd { get; set; } = new List<string>();
        public List<string> RolesToRemove { get; set; } = new List<string>();
    }

    public class RoleDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
    }
}