using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public interface IPaymentAccountStateValidator
    {
        MakePaymentResult Validate(MakePaymentRequest request, Account account);
    }
}