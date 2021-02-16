using AutoFixture;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.ServicesTests
{
    [TestFixture]
    public class ChapsPaymentValidationStrategyTests
    {
        private IPaymentValidationStrategy _sut;
        Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _sut = new ChapsPaymentValidationStrategy();
        }
        [Test]
        public void Validate_Should_Fail_When_Account_IsNotLive()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            var account = _fixture.Create<Account>();
            account.Status = AccountStatus.Disabled;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;
    
            var result = _sut.Validate(account, request);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.False);
        }
        
        [Test]
        public void Validate_Should_Fail_When_Account_HasNoFlag_For_ChapsPaymentScheme()
        {
            var account = _fixture.Create<Account>();
            var request = _fixture.Create<MakePaymentRequest>();
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;

            var result = _sut.Validate(account, request);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.False);
        }
        
        [Test]
        public void Validate_Should_Succeed_When_Account_HasFlag_For_ChapsPaymentScheme()
        {
            var account = _fixture.Create<Account>();
            var request = _fixture.Create<MakePaymentRequest>();
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;

            var result = _sut.Validate(account, request);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.True);
        }
    }
}