namespace Domain.Interfaces.Service;

public interface ITransacao : IDisposable
{
    void Gravar();
}

public interface ITransacaoFactory
{
    ITransacao CriaTransacao();
}
