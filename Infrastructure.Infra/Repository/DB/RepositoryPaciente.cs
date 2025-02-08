using Domain.Entity;
using Domain.Interfaces.Repository;
using System.Data;

namespace Infrastructure.Repository.DB;

public class RepositoryPaciente(IDbTransaction dbTransaction) : IRepositoryPaciente
{
    public Task AlterarDadosPaciente(Paciente paciente)
    {
        throw new NotImplementedException();
    }

    public Task ExcluirPaciente(int id)
    {
        throw new NotImplementedException();
    }

    public Task RegistarNovoPaciente(Paciente paciente)
    {
        throw new NotImplementedException();
    }

    public Task<Paciente?> ResgatarPacientePorEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Paciente?> ResgatarPacientePorId(int id)
    {
        throw new NotImplementedException();
    }
}
