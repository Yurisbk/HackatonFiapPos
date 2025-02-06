using AgendamentoConsultasMedicas;
using Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace contatos_testes_integration.fixture;

public class WebAppFixture
{
    public HttpClient TestHtppClient { get; }
    public IServiceProvider ServiceProvider { get; }
    public IConfiguration Configuration { get; }

    public WebAppFixture()
    {
        var factory = new WebApplicationFactory<Program>();
        TestHtppClient = factory.CreateClient();
        ServiceProvider = factory.Services.CreateScope().ServiceProvider;
        Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
    }
}

public class TestBaseWebApp(WebAppFixture webAppFixture) : IClassFixture<WebAppFixture>
{
    protected HttpClient HttpClient => webAppFixture.TestHtppClient;
    protected IServiceProvider ServiceProvider => webAppFixture.ServiceProvider;
    protected IConfiguration Configuration => webAppFixture.Configuration;
}