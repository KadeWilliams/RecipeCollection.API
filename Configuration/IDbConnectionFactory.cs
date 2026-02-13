using Npgsql;
using System.Data;

namespace RecipeCollection.API.Configuration;

public interface IDbConnectionFactory
{
    IDbConnection GetConnection();
}
public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    public NpgsqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    public IDbConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
