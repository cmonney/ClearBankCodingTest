using AutoFixture;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.DataTests
{
    [TestFixture]
    public class BackupAccountDataStoreTests
    {
        private IAccountDataStore _sut;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _sut = new BackupAccountDataStore();
        }

        [Test]
        public void GetAccount_Should_Return_An_Account()
        {
            var accountNumber = _fixture.Create<string>();
            
            var account = _sut.GetAccount(accountNumber);

            Assert.IsNotNull(account);
            Assert.IsInstanceOf<Account>(account);
        }

        [Test]
        public void UpdateAccount_Should_Not_Throw_Errors()
        {
            var account = _fixture.Create<Account>();
            
            Assert.DoesNotThrow(() => _sut.UpdateAccount(account));
        }
    }
}