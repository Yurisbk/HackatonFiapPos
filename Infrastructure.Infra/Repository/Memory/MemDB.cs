using Domain.Entity;
using Domain.Interfaces.Repository;
using System.Data;

namespace Infrastructure.Repository.Memory;

public static class MemDB
{
    static public List<Paciente> Pacientes { get; } = new();
    static public List<Medico> Medicos { get; } = new();
    static public List<HorarioMedico> HorariosMedicos { get; } = new();
    static public List<Consulta> Consultas { get; } = new();

    static public int CriaChaveUnica<T>(List<T> lista) where T: Entidade
    {
        if (lista.Count == 0)
            return 0;

        return lista.Max(e => e.Id ?? 0) + 1;
    }

    static public void SimulaUK<T>(List<T> lista, Func<T, T, bool> condicao, T entidade, string mensagem) where T: Entidade
    {
        if (lista.Any(t => condicao(t, entidade)))
            throw new InvalidOperationException(mensagem);
    }
}