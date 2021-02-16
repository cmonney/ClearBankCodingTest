using AutoFixture;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.ServicesTests
{
    [TestFixture]
    public class PaymentAccountStateValidatorTests
    {
        private IPaymentAccountStateValidator _sut;
        private Fixture _fixture;
        
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _sut = new PaymentAccountStateValidator();
        }

        [Test]
        public void Validate_Should_Return_Success_False_When_Account_Is_Null()
        {
            var request = _fixture.Create<MakePaymentRequest>();

            var result = _sut.Validate(request, null);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void Validate_Should_Return_Success_False_When_RequestPaymentScheme_Is_Bacs_And_Account_HasNoFlag()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            request.PaymentScheme = PaymentScheme.Bacs;
            var account = _fixture.Create<Account>();
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            
            var result = _sut.Validate(request, account);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.False);
        }
        
        [Test]
        public void Validate_Should_Return_Success_False_When_RequestPaymentScheme_Is_FasterPayments_And_Account_HasNoFlag()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            request.PaymentScheme = PaymentScheme.FasterPayments;
            var account = _fixture.Create<Account>();
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            
            var result = _sut.Validate(request, account);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.False);
        }
        
        [Test]
        public void Validate_Should_Return_Success_False_When_RequestPaymentScheme_Is_Chaps_And_Account_HasNoFlag()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            request.PaymentScheme = PaymentScheme.Chaps;
            var account = _fixture.Create<Account>();
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;
            
            var result = _sut.Validate(request, account);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.False);
        }
        
        [Test]
        public void Validate_Should_Return_Success_False_When_RequestPaymentScheme_Is_Chaps_And_Account_Is_Not_Live()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            request.PaymentScheme = PaymentScheme.Chaps;
            var account = _fixture.Create<Account>();
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            account.Status = AccountStatus.Disabled;
            
            var result = _sut.Validate(request, account);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.False);
        }
        
        [Test]
        public void Validate_Should_Return_Success_True_When_RequestPaymentScheme_Is_Chaps_And_Account_Is_Live()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            request.PaymentScheme = PaymentScheme.Chaps;
            var account = _fixture.Create<Account>();
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            account.Status = AccountStatus.Live;
            
            var result = _sut.Validate(request, account);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.True);
        }
        
        [Test]
        public void Validate_Should_Return_Success_False_When_RequestPaymentScheme_Is_FasterPayments_And_Account_Balance_Is_Less_Than_Request_Amount()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            request.Amount = (decimal) 100.0;
            request.PaymentScheme = PaymentScheme.FasterPayments;
            var account = _fixture.Create<Account>();
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            account.Balance = (decimal) 90.0;
            
            var result = _sut.Validate(request, account);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.False);
        }
        
        [Test]
        public void Validate_Should_Return_Success_True_When_RequestPaymentScheme_Is_FasterPayments_And_Account_Balance_Is_More_Than_Request_Amount()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            request.Amount = (decimal) 100.0;
            request.PaymentScheme = PaymentScheme.FasterPayments;
            var account = _fixture.Create<Account>();
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            account.Balance = (decimal) 190.0;
            
            var result = _sut.Validate(request, account);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.True);
        }
        
        [Test]
        public void Validate_Should_Return_Success_True_When_RequestPaymentScheme_Is_FasterPayments_And_Account_Balance_Is_Equal_To_Request_Amount()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            request.Amount = (decimal) 100.0;
            request.PaymentScheme = PaymentScheme.FasterPayments;
            var account = _fixture.Create<Account>();
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            account.Balance = (decimal) 100.0;
            
            var result = _sut.Validate(request, account);

            Assert.IsNotNull(result);
            Assert.That(result.Success, Is.True);
        }
    }
}