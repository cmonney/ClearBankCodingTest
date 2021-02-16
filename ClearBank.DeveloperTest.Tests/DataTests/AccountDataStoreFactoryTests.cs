using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.DataTests
{
    [TestFixture]
    public class AccountDataStoreFactoryTests
    {
        private const string Key = "DataStoreType";
        private IAccountDataStoreFactory _sut;
        private Mock<IAppSettings> _appSettings;

        [SetUp]
        public void Initialize()
        {
            _appSettings = new Mock<IAppSettings>();
            _sut = new AccountDataStoreFactory(_appSettings.Object);
        }

        [Test]
        public void Factory_Should_Return_BackupAccountDataStore_When_DataStoreTypeIsBackup()
        {
            _appSettings
                .Setup(x => x.GetDataStoreTypeConfigValue(Key))
                .Returns("Backup");

            var dataStore = _sut.Create();

            Assert.That(dataStore, Is.InstanceOf<BackupAccountDataStore>());
            _appSettings.Verify(x => x.GetDataStoreTypeConfigValue(Key));
        }

        [Test]
        public void Factory_Should_Return_AccountDataStore_When_DataStoreTypeIsNotBackup()
        {
            _appSettings
                .Setup(x => x.GetDataStoreTypeConfigValue(Key))
                .Returns("Something");

            var dataStore = _sut.Create();

            Assert.That(dataStore, Is.InstanceOf<AccountDataStore>());
            _appSettings.Verify(x => x.GetDataStoreTypeConfigValue(Key));
        }
    }
}