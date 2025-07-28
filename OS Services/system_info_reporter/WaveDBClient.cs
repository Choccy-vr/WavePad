using Tmds.DBus;
using System.Text.Json;
using System.Threading.Tasks;

public class WaveDBClient
{
    private Connection? _connection;
    private IWaveDBService? _proxy;

    public async Task ConnectAsync()
    {
        if (_connection != null) throw new InvalidOperationException("Not connected to D-Bus service.");

        _connection = new Connection(Address.Session);
        await _connection.ConnectAsync();
        _proxy = _connection.CreateProxy<IWaveDBService>("org.waveOS.WaveDB", "/org/waveOS/WaveDB");
        if (_proxy == null)
        {
            throw new InvalidOperationException("Failed to create proxy for WaveDB service.");
        }
    }

    public Task<string> WriteRowAsync(string database, string table, object data)
    {
        if (_proxy == null) throw new InvalidOperationException("Not connected to D-Bus service.");
        string jsonData = JsonSerializer.Serialize(data);
        return _proxy.WriteRowAsync(database, table, jsonData);
    }

    public Task<string> CreateAsync(string database, string table, object columns)
    {
        if (_proxy == null) throw new InvalidOperationException("Not connected to D-Bus service.");
        string jsonColumns = JsonSerializer.Serialize(columns);
        return _proxy.CreateAsync(database, table, jsonColumns);
    }

    // Add other methods (Read, Delete, etc.) as needed...
}