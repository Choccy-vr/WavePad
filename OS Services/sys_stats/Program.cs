using Tmds.DBus;

namespace sys_stats
{
    class Program
    {
        static async void Main(string[] args)
        {
            try
            {
                var connection = new Connection(Address.Session);
                await connection.ConnectAsync();

                var service = new SysStatsService();
                await connection.RegisterObjectAsync(service);
                await connection.RegisterServiceAsync("org.waveOS.SysStats");

                // Keep the service running
                await Task.Delay(-1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start SysStats service: {ex.Message}");
            }
        }
    }
}