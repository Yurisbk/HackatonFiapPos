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

    static public int PK<T>(this List<T> lista) where T : Entidade
    {
        if (lista.Count == 0)
            return 0;

        return lista.Max(e => e.Id ?? 0) + 1;
    }

    static public void PK<T>(this List<T> lista, T entidade) where T : Entidade =>
        entidade.Id = lista.PK();

    static public void UK<T>(this List<T> lista, Func<T, bool> condicao, string mensagem) where T: Entidade
    {
        if (lista.Any(condicao))
            throw new InvalidOperationException(mensagem);
    }

    static public void FK<T>(this List<T> lista, int? id, string mensagem) where T: Entidade
    {
        if (!lista.Any(t => t.Id == id))
            throw new InvalidOperationException(mensagem);
    }

    static public int IndexOfEntity<T>(this List<T> lista, T entidade) where T : Entidade =>
        lista.FirstOrDefault(i => i.Id == entidade.Id)?.Id ?? -1;

    static public int IndexOfId<T>(this List<T> lista, int id) where T : Entidade =>
        lista.FirstOrDefault(i => i.Id == id)?.Id ?? -1;
}