using Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class DTOCreatePaciente
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
        public virtual void Validar()
        {
            if (!Validacoes.ValidarCPF(CPF))
                throw new Exception("CPF inválido");
        }
    }
}
