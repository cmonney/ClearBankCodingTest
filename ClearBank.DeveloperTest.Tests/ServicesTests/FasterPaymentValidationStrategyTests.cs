using AutoFixture;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.ServicesTests
{
    [TestFixture]
    public class FasterPaymentValidationStrategyTests
    {
        private IPaymentValidationStrategy _sut;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _sut = new FasterPaymentValidationStrategy();
        }
        
        [Test]
        public void Validate_Should_Fail_When_Account_HasNoFlag_For_FasterPaymentScheme()
        {
            var account = _fixture.Create<Account>();
            var request = _fixture.Create<MakePaymentRequest>();
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;

            var result = _sut.Validate(account, request);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.False);
        }
        
        [Test]
        public void Validate_Should_Succeed_When_Account_HasFlag_For_FasterPaymentScheme()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            request.Amount = (decimal) 50.0;
            var account = _fixture.Create<Account>();
            account.Balance = request.Amount + (decimal) 1.0;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;

            var result = _sut.Validate(account, request);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.True);
        }
        
        [Test]
        public void Validate_Should_Fail_When_Account_Balance_Is_Less_Than_Requested_Amount()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            request.Amount = (decimal) 50.0;
            var account = _fixture.Create<Account>();
            account.Balance = request.Amount - (decimal) 5.0;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;

            var result = _sut.Validate(account, request);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.False);
        }
    }
}