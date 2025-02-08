using System.Data;

namespace Infrastructure.Repository.DB;

internal class RepositoryConsulta
{
    private readonly IDbConnection _dbConnection;

    public RepositoryConsulta(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
}
