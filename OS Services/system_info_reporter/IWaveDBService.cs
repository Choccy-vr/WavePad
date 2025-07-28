using Tmds.DBus;
using System.Threading.Tasks;

[DBusInterface("org.waveOS.WaveDB")]
public interface IWaveDBService : IDBusObject
{
    Task<string> WriteAsync(string databaseName, string tableName, string data);
    Task<string> ReadAsync(string databaseName, string tableName, string[]? items, string? where, string? order);
    Task<string> ReadRowsAsync(string databaseName, string tableName);
    Task<string> DeleteAsync(string databaseName, string tableName, string whereClause);
    Task<string> CreateAsync(string databaseName, string tableName, string columns);
    Task<string> WriteRowAsync(string databaseName, string tableName, string data);
}