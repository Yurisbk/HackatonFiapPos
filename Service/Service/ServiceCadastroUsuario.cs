using Domain.DTO.Autenticacao;
using Domain.DTO;
using System.Text;
using System.Text.Json;
using Domain.Interfaces.Repository;

namespace Service.Service;

public class ServiceCadastroUsuario : IServiceCadastroUsuario
{
    private readonly HttpClient _httpClient;

    public ServiceCadastroUsuario(IHttpClientFactory httpClientFactory, IRepositoryPaciente repositorioPaciente)
    {
        _httpClient = httpClientFactory.CreateClient("AutenticacaoAPI");
    }
    public async Task<DTOAutenticacaoResponse> RealizarLogin(DTOLoginUsuario loginUsuario)
    {
        string jsonContent = JsonSerializer.Serialize(loginUsuario);
        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync("api/account/login", content);

        if (!response.IsSuccessStatusCode)
        {
            string errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro na requisição: {response.StatusCode} - {errorResponse}");
        }

        string responseBody = await response.Content.ReadAsStringAsync();
        DTOAutenticacaoResponse authResponse = JsonSerializer.Deserialize<DTOAutenticacaoResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return authResponse;
    }
    public async Task<DTOCreateUsuarioResponse> CriarUsuario(DTOCreatePaciente createPaciente)
    {
        DTOCreateUsuario usuario = new DTOCreateUsuario()
        {
            Email = createPaciente.Email,
            Password = createPaciente.Senha,
            Perfis = new List<UserRoles> { UserRoles.Paciente }
        };
        string jsonContent = JsonSerializer.Serialize(usuario);
        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync("api/account/register", content);

        if (!response.IsSuccessStatusCode)
        {
            string errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro na requisição: {response.StatusCode} - {errorResponse}");
        }

        string responseBody = await response.Content.ReadAsStringAsync();
        DTOCreateUsuarioResponse authResponse = JsonSerializer.Deserialize<DTOCreateUsuarioResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return authResponse;
    }
}
