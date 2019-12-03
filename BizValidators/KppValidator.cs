using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using StringUsefullExtensions;

namespace BizValidators
{
    public class KppValidator : IValidator

    {
        public Result Validate(string value)
        {
            return ValidateAsync(value).GetAwaiter().GetResult();
        }

        public Task<Result> ValidateAsync(string value)
        {
            if (value.IsNullOrEmpty())
            {
                return Task.FromResult(Result.Fail("Kpp must have a value"));
            }

            var lengthCheck = LengthCheck(value);
            var firstSymbolCheck = FistSymbolDigitsCheck(value);
            var basicCheck = Result.Combine(lengthCheck, firstSymbolCheck); 
            
            if (basicCheck.IsFailure)
            {
                return Task.FromResult<Result>(basicCheck);
            }
            
            var middleSymbolsCheck = MiddleSymbolsCheck(value);
            
            return Task.FromResult<Result>(Result.Combine(basicCheck, middleSymbolsCheck));
        }

        private Result LengthCheck(string value)
        {
            if (value.Length == 9)
            {
                return Result.Ok();
            }
            
            return Result.Fail("Kpp must have 9 symbols");
        }

        private Result FistSymbolDigitsCheck(string value)
        {
            if (value.Take(4).AllDigits())
            {
                return Result.Ok();
            }
            
            return Result.Fail("Kpp 4 symbols must be a digits");
        }

        private Result MiddleSymbolsCheck(string value)
        {
            if (value.Skip(6).AllDigits() &&
                ((value[4].IsDigit() && value[5].IsDigit() &&
                  !(value[4] == '0' && value[5] == '0')) ||
                 (value[4].IsLatinLetter() && value[5].IsLatinLetter())))
            {
                return Result.Ok();
            }
            
            return Result.Fail("Kpp must have special 4 and 5 symbols");
        }
    }
}