using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class DTOCreateMedico : DTOCreatePessoa
    {
        [Required]
        public string CRM { get; set; }
        [Required]
        public string? Especialidade { get; set; }
        [Required]
        public double ValorConsulta { get; set; }
    }
}
