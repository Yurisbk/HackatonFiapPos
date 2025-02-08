using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class DTONotificacao
    {
        public string? EmailMedico { get; set; }

        public string? EmailPaciente { get; set; }

        public string? NomePaciente { get; set; }

        public string? NomeMedico { get; set; }

        public string? HorarioConsulta { get; set; }

        public bool? Confirmacao { get; set; }
    }
}
