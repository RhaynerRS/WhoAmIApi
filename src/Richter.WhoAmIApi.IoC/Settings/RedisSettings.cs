namespace Richter.WhoAmIApi.IoC.Settings
{
    public class RedisSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int DatabaseIndex { get; set; } = 0;
        public int ConnectTimeout { get; set; } = 5000;
        public int SyncTimeout { get; set; } = 5000;
    }
}
