namespace Domain.Interfaces.Repository;

public interface ITransacao: IDisposable
{
    void Gravar();
}
