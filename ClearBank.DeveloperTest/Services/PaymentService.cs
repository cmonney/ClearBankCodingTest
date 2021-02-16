using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IPaymentAccountStateValidator _accountStateValidator;

        public PaymentService(IAccountDataStoreFactory accountDataStoreFactory,
            IPaymentAccountStateValidator accountStateValidator)
        {
            _accountStateValidator = accountStateValidator;
            _accountDataStore = accountDataStoreFactory.Create();
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);
            var result = _accountStateValidator.Validate(request, account);

            if (result.Success)
            {
                account.Balance -= request.Amount;
                _accountDataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}