using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer_Notificacao.Entity
{
    public class Notificacao
    {
        public string? EmailPaciente { get; set; }

        public string? NomePaciente { get; set; }

        public string? NomeMedico { get; set; }

        public string? HorarioConsulta { get; set; }
    }
}
