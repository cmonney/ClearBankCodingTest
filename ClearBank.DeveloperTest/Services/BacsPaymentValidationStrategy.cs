using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class BacsPaymentValidationStrategy : IPaymentValidationStrategy
    {
        public MakePaymentResult Validate(Account account, MakePaymentRequest request)
        {
            var result = new MakePaymentResult();
            
            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
            {
                result.Success = false;
            }
            else
            {
                result.Success = true;
            }

            return result;
        }
    }
}