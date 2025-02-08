using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.Autenticacao;

public class DTOLoginUsuario
{
    [Required]
    [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
    public string? Email { get; set; }   
    public string Password { get; set; }
}
