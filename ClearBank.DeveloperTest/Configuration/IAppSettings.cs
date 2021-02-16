using System.Configuration;

namespace ClearBank.DeveloperTest.Configuration
{
    public interface IAppSettings
    {
        string GetDataStoreTypeConfigValue(string key);
    }

    public class AppSettings : IAppSettings
    {
        public string GetDataStoreTypeConfigValue(string key)
        {
            var value = ConfigurationManager.AppSettings[key] ?? "Backup";
            return value;
        }
    }
}