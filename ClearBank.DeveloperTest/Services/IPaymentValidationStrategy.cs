using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public interface IPaymentValidationStrategy
    {
        MakePaymentResult Validate(Account account, MakePaymentRequest request);
    }
}