using Domain.Interfaces.Repository;
using System.Data;

namespace Infrastructure.Repository.DB;

public class DBTransacao : ITransacao
{
    IDbTransaction? dbTransaction;

    public DBTransacao(IDbConnection? dbConnection) 
        => dbTransaction = dbConnection?.BeginTransaction();

    public void Commit() 
        => dbTransaction?.Commit();

    public void Dispose()
    {
        dbTransaction?.Rollback();
        dbTransaction?.Dispose();
    }
}
