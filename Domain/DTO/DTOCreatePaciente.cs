using Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class DTOCreatePaciente : DTOCreatePessoa
    {        
        public virtual void Validar()
        {
            if (!Validacoes.ValidarCPF(CPF))
                throw new Exception("CPF inválido");
        }
    }
}
