using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class DTOLoginPaciente
    {
        [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
        public string? Email { get; set; }
        public string? Cpf { get; set; }
        public string Password { get; set; }
    }
}
