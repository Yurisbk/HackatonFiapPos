using Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Service.Helper;

public class HelperTransacao(IServiceProvider serviceProvider)
{
    public ITransacao CriaTransacao() => serviceProvider.GetService<ITransacao>()!;
}
