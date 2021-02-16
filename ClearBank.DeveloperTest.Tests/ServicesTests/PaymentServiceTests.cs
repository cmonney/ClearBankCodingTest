using AutoFixture;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.ServicesTests
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private IPaymentService _sut;
        private Mock<IAccountDataStoreFactory> _accountDataStoreFactory;
        private Mock<IAccountDataStore> _accountDataStore;
        private Mock<IPaymentAccountStateValidator> _accountStateValidator;
        private Fixture _fixture;
        
        [SetUp]
        public void Init()
        {
            _fixture = new Fixture();
            _accountDataStore = new Mock<IAccountDataStore>();
            _accountStateValidator = new Mock<IPaymentAccountStateValidator>();
            _accountDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            _accountDataStoreFactory
                .Setup(x => x.Create())
                .Returns(_accountDataStore.Object);
            _sut = new PaymentService(_accountDataStoreFactory.Object, _accountStateValidator.Object);
        }

        [Test]
        public void Service_Should_Lookup_Payment_Account()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            var account = _fixture.Create<Account>();
            
            _accountStateValidator
                .Setup(x => x.Validate(request, account))
                .Returns(new MakePaymentResult());
            
            _accountDataStore
                .Setup(x => x.GetAccount(request.DebtorAccountNumber))
                .Returns(account);

            var result = _sut.MakePayment(request);

            Assert.IsNotNull(result);

            _accountDataStore.Verify(x => x.GetAccount(request.DebtorAccountNumber), Times.Once);
            _accountDataStoreFactory.VerifyAll();
        }
        
        [Test]
        public void Service_Should_Validate_Payment_Account_State()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            var account = _fixture.Create<Account>();
            var expectedResult = _fixture.Create<MakePaymentResult>();

            _accountStateValidator
                .Setup(x => x.Validate(request, account))
                .Returns(expectedResult);
            
            _accountDataStore
                .Setup(x => x.GetAccount(request.DebtorAccountNumber))
                .Returns(account);

            var result = _sut.MakePayment(request);

            Assert.IsNotNull(result);
            Assert.AreSame(expectedResult, result);

            _accountStateValidator.Verify(x => x.Validate(request, account), Times.Once);
        }

        [Test]
        public void Service_Should_Deduct_Payment_From_Account_Balance_When_Account_Is_In_Valid_State()
        {
            var balance = _fixture.Create<decimal>();
            var amount = _fixture.Create<decimal>();
            var accountBalance = amount + balance;
            
            var request = _fixture.Create<MakePaymentRequest>();
            request.Amount = amount;
            
            var account = _fixture.Create<Account>();
            account.Balance = accountBalance;
            
            var expectedResult = _fixture.Create<MakePaymentResult>();
            expectedResult.Success = true;

            _accountStateValidator
                .Setup(x => x.Validate(request, account))
                .Returns(expectedResult);
            
            _accountDataStore
                .Setup(x => x.GetAccount(request.DebtorAccountNumber))
                .Returns(account);
            
            _accountDataStore
                .Setup(x => x.UpdateAccount(It.IsAny<Account>()))
                .Verifiable();

            var result = _sut.MakePayment(request);

            Assert.IsNotNull(result);
            Assert.AreSame(expectedResult, result);

            _accountDataStore.Verify(x => x.GetAccount(request.DebtorAccountNumber), Times.Once);
            _accountDataStore.Verify(x => x.UpdateAccount(It.Is<Account>(a => a.Balance == balance)), Times.Once);
        }
        
        [Test]
        public void Service_Should_Not_Deduct_Payment_From_Account_Balance_When_Account_IsNot_In_Valid_State()
        {
            var request = _fixture.Create<MakePaymentRequest>();
            var account = _fixture.Create<Account>();
            var expectedResult = _fixture.Create<MakePaymentResult>();
            expectedResult.Success = false;

            _accountStateValidator
                .Setup(x => x.Validate(request, account))
                .Returns(expectedResult);
            
            _accountDataStore
                .Setup(x => x.GetAccount(request.DebtorAccountNumber))
                .Returns(account);

            var result = _sut.MakePayment(request);
            
            Assert.IsNotNull(result);
            Assert.AreSame(expectedResult, result);
            
            _accountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }
    }
}   