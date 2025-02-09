using Domain.Entity;
using Domain.Interfaces.Service;
using Force.DeepCloner;
using System.Data;

namespace Infrastructure.Repository.Memory;

public static class MemDB
{
    static public List<Paciente> Pacientes { get; } = new();
    static public List<Medico> Medicos { get; } = new();
    static public List<HorarioMedico> HorariosMedicos { get; } = new();
    static public List<Consulta> Consultas { get; } = new();

    static public SemaphoreSlim DBLock { get; } = new(1, 1);

    static public int GetPK<T>(this List<T> lista) where T : Entidade
    {
        if (lista.Count == 0)
            return 0;

        return lista.Max(e => e.Id ?? 0) + 1;
    }

    static public void GetPK<T>(this List<T> lista, T entidade) where T : Entidade =>
        entidade.Id = lista.GetPK();

    static public void CheckUK<T>(this List<T> lista, Func<T, bool> condicao, string mensagem) where T: Entidade
    {
        if (lista.Any(condicao))
            throw new InvalidOperationException(mensagem);
    }

    static public void CheckFK<T>(this List<T> lista, int? id, string mensagem) where T: Entidade
    {
        if (!lista.Any(t => t.Id == id))
            throw new InvalidOperationException(mensagem);
    }

    static public int IndexOfEntity<T>(this List<T> lista, T entidade) where T : Entidade =>
        lista.FirstOrDefault(i => i.Id == entidade.Id)?.Id ?? -1;

    static public int IndexOfId<T>(this List<T> lista, int id) where T : Entidade =>
        lista.FirstOrDefault(i => i.Id == id)?.Id ?? -1;

    static public T Insert<T>(this List<T>lista, T entidade) where T : Entidade
    {
        lista.GetPK(entidade);
        lista.Add(entidade.DeepClone());
        return entidade;
    }

    static public void Update<T>(this List<T> lista, T entidade) where T : Entidade 
        => lista[lista.IndexOfEntity(entidade)] = entidade.DeepClone();

    static public void Delete<T>(this List<T> lista, int id) where T : Entidade 
        => lista.RemoveAt(lista.IndexOfId(id));
}

public class TransacaoFakeMemoria : ITransacao
{
    public void Gravar()
    {

    }

    public void Dispose()
    {
        
    }
}