using contatos_testes_integration.fixture;
using Domain.Entity;
using Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration.Infrastructure;

public class TestsRepositoryConsuta(WebAppFixture webAppFixture) : TestBaseWebApp(webAppFixture)
{
    [Fact]
    public void TestRegistrarConsulta()
    {
        
    }
}
