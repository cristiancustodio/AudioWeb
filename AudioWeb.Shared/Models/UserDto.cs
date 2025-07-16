namespace AudioWeb.Shared.Models
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NomeCompleto { get; set; }
        public List<string> Roles { get; set; } = new List<string>(); // Papéis do usuário
    }

    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NomeCompleto { get; set; }
        public string SelectedRole { get; set; } // Para o administrador selecionar
    }

    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; }
        public List<string> RolesToAdd { get; set; } = new List<string>();
        public List<string> RolesToRemove { get; set; } = new List<string>();
    }

    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}