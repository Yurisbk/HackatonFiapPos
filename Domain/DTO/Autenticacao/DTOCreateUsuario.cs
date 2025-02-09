using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.Autenticacao;

public class DTOCreateUsuario
{
    [Required]
    [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
    public string Email { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    public string Password { get; set; }
    [Required]
    public List<UserRoles> Perfis { get; set; }
}

public enum UserRoles
{
    Medico,
    Paciente
}
