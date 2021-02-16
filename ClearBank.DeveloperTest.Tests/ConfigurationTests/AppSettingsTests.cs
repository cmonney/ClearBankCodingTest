using ClearBank.DeveloperTest.Configuration;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.ConfigurationTests
{
    [TestFixture]
    public class AppSettingsTests
    {
        private IAppSettings _sut;
        
        [SetUp]
        public void Setup()
        {
            _sut = new AppSettings();
        }
        
        [Test]
        public void AppSettings_Should_DefaultValue()
        {
            const string key = "DataStoreType";
            const string expectedValue = "Backup";

            var result = _sut.GetDataStoreTypeConfigValue(key);

            Assert.AreEqual(expectedValue, result);
        }
    }
}