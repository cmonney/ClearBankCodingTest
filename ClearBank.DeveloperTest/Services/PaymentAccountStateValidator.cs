using System.Collections.Generic;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentAccountStateValidator : IPaymentAccountStateValidator
    {
        private readonly Dictionary<PaymentScheme, IPaymentValidationStrategy> _strategyContext =
            new Dictionary<PaymentScheme, IPaymentValidationStrategy>();

        public PaymentAccountStateValidator()
        {
            _strategyContext.Add(PaymentScheme.Bacs, new BacsPaymentValidationStrategy());
            _strategyContext.Add(PaymentScheme.Chaps, new ChapsPaymentValidationStrategy());
            _strategyContext.Add(PaymentScheme.FasterPayments, new FasterPaymentValidationStrategy());
        }

        public MakePaymentResult Validate(MakePaymentRequest request, Account account)
        {
            if (account == null) return new MakePaymentResult {Success = false};

            var validationStrategy = _strategyContext[request.PaymentScheme];
            return validationStrategy.Validate(account, request);
        }
    }
}