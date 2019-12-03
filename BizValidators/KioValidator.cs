using System.Threading.Tasks;
using BizValidators;
using CSharpFunctionalExtensions;
using StringUsefullExtensions;

namespace Core
{
    public class KioValidator : IValidator
    {
        public Result Validate(string value)
        {
            return ValidateAsync(value).GetAwaiter().GetResult();
        }

        public Task<Result> ValidateAsync(string value)
        {
            if (value.IsNullOrEmpty())
            {
                return Task.FromResult<Result>(Result.Fail("Kio must have a value"));
            }

            var lengthCheck = LengthCheck(value);
            var digitsCheck = DigitsOnlyCheck(value);

            return Task.FromResult(Result.Combine(lengthCheck, digitsCheck));
        }

        private Result LengthCheck(string value)
        {
            if (value.Length == 5)
            {
                return Result.Ok();
            }
            
            return Result.Fail("Kio must have 5 symbols");
        }

        private Result DigitsOnlyCheck(string value)
        {
            if (value.AllDigits())
            {
                return Result.Ok();
            }
            
            return Result.Fail("Kio must have only digits");
        }
    }
}