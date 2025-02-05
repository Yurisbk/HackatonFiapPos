using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class DTOCreatePessoa
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string CPF { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}