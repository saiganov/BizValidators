using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using StringUsefullExtensions;

namespace BizValidators
{
    public class BankingLicenseValidator : IValidator
    {
        private static readonly char[] EndLetter = { 'K', 'k', 'К', 'к', 'Д', 'д' };
        
        public Result Validate(string value)
        {
            return ValidateAsync(value).GetAwaiter().GetResult();
        }
        
        public Task<Result> ValidateAsync(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Task.FromResult<Result>(Result.Fail("Banking Licence must have value"));
            }

            if (value.Length == 4)
            {
                return Task.FromResult<Result>(Check4Symbols(value));
            }
            if(value.Length == 6)
            {
                return Task.FromResult<Result>(Check6Symbols(value));
            }
            if(value.Length == 7)
            {
                return Task.FromResult<Result>(Check7Symbols(value));
            }
            
            return Task.FromResult<Result>(Result.Fail("Licence must have 4 or 6 or 7 symbols"));
        }

        private Result Check4Symbols(string value)
        {
            if (value.AllDigits())
            {
                return Result.Ok();
            }
            return Result.Fail("Bankincg licence must contains only digits");
        }
        
        private Result Check6Symbols(string value)
        {
            var digitsCheck = value.Take(4).AllDigits() ? Result.Ok() : Result.Fail("Banking licence must be digits from 0 to 4");
            var dashCheck = value[4] == '-' ? Result.Ok() : Result.Fail("Banking licence 5 symbol must be -");
            var endLetterCheck = EndLetter.Contains(value[5]) ? Result.Ok() : Result.Fail($"Banking licens end letter must be in {EndLetter}");

            return Result.Combine(digitsCheck, dashCheck, endLetterCheck);
        }
        
        private Result Check7Symbols(string value)
        {
            var digitsCheck = value.Take(4).AllDigits() ? Result.Ok() : Result.Fail("Banking licence must be digits from 0 to 4");
            var endWithCheck = value.EndsWith("-ЦК", StringComparison.InvariantCultureIgnoreCase)
                ? Result.Ok()
                : Result.Fail("Banking licens end letter must be in -ЦК");
            
            return Result.Combine(digitsCheck, endWithCheck);
        }
    }
}