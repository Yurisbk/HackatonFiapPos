using Domain.Interfaces.Service;

namespace AgendamentoConsultasMedicas.Configuration;

public class TransacaoFactory(IServiceProvider serviceProvider) : ITransacaoFactory
{
    public ITransacao CriaTransacao() => serviceProvider.GetService<ITransacao>()!;
}