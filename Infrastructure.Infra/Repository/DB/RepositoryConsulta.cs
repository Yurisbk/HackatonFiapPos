using Dapper;
using Domain.Entity;
using Domain.Interfaces.Repository;
using System.Data;

namespace Infrastructure.Repository.DB;

internal class RepositoryConsulta : IRepositoryConsulta
{
    private readonly IDbConnection _dbConnection;

    public RepositoryConsulta(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Consulta[]> ListarProximasConsultas(int dias = 15)
    {
        string query = "SELECT * FROM Consulta WHERE DataConsulta >= CURRENT_DATE AND DataConsulta <= CURRENT_DATE + INTERVAL '@Dias days'";
        var consultas = (await _dbConnection.QueryAsync<Consulta>(query, new { Dias = dias })).ToArray();
        return consultas;
    }

    public async Task RegistrarConsulta(Consulta consulta)
    {
        throw new NotImplementedException();
    }
}
