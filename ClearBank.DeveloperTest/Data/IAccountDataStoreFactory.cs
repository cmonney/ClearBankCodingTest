using ClearBank.DeveloperTest.Configuration;

namespace ClearBank.DeveloperTest.Data
{
    public interface IAccountDataStoreFactory
    {
        IAccountDataStore Create();
    }

    public class AccountDataStoreFactory : IAccountDataStoreFactory
    {
        private const string Key = "DataStoreType";
        private readonly IAppSettings _appSettings;

        public AccountDataStoreFactory(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public IAccountDataStore Create()
        {
            if (_appSettings.GetDataStoreTypeConfigValue(Key) == "Backup")
            {
                return new BackupAccountDataStore();
            }

            return new AccountDataStore();
        }
    }
}