using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using StringUsefullExtensions;

namespace BizValidators
{
    public class BicValidator : IValidator
    {   
        public Result Validate(string value)
        {
            return ValidateAsync(value).GetAwaiter().GetResult();
        }

        public Task<Result> ValidateAsync(string value)
        {
            if (value.IsNullOrEmpty())
            {
                return Task.FromResult(Result.Fail("Bic must have value"));
            }
            
            var result = Result.Combine(LengthCheck(value), StartWithCheck(value), AllDigits(value));
            
            return Task.FromResult(result);
        }
        
        
        private Result LengthCheck(string val)
        {
            return val.Length != 9 ? Result.Fail("Bic length must be 9 symbols") : Result.Ok();
        }
        private Result StartWithCheck(string val)
        {
            return !val.StartsWith("04") ? Result.Fail("Bic must be start with 04") : Result.Ok();
        }
        private Result AllDigits(string val)
        {
            return val.AllDigits() ? Result.Ok() : Result.Fail("Bic must be contains only digits");
        }
    }
}
