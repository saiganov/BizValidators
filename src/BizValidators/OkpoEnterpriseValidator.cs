using System.Linq;
using System.Threading.Tasks;
using Core.Utils;
using CSharpFunctionalExtensions;
using StringUsefullExtensions;

namespace BizValidators
{
    public class OkpoEnterpriseValidator : IValidator
    {
        public Result Validate(string value)
        {
            return ValidateAsync(value).GetAwaiter().GetResult();
        }

        public Task<Result> ValidateAsync(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Task.FromResult<Result>(Result.Fail("Okpo must have a value"));
            }

            var lengthCheck = LengthCheck(value);
            var digitsCheck = DigitCheck(value);
            var basicCheck = Result.Combine(lengthCheck, digitsCheck);
            if (basicCheck.IsFailure)
            {
                return Task.FromResult<Result>(basicCheck);
            }

            var controlSumCheck = ControlSumCheck(value);
            return Task.FromResult(Result.Combine(basicCheck, controlSumCheck));
        }

        private Result LengthCheck(string value)
        {
            if (value.Length == 8)
            {
                return Result.Ok();
            }
            
            return Result.Fail("Okpo ust be 8 symbols");
        }

        private Result DigitCheck(string value)
        {
            if (value.AllDigits())
            {
                return Result.Ok();
            }
            
            return Result.Fail("Okpo must contains only digits");
        }

        private Result ControlSumCheck(string value)
        {
            var calculation = value.Take(7).Select(
                                      (c, i) => (i + 1) * c.GetIntFromDigitChar())
                                  .Sum() % 11;
            if (calculation != 10)
            {
                if (calculation == value[7].GetIntFromDigitChar())
                {
                    return Result.Ok();
                }
            }
            else
            {
                calculation = value.Take(7).Select(
                                      (c, i) => (i + 3) * c.GetIntFromDigitChar())
                                  .Sum() % 11;
                if ((calculation == 10 ? 0 : calculation) == value[7].GetIntFromDigitChar())
                {
                    return Result.Ok();
                }
            }
            
            return Result.Fail("Control sum not valid");
        }
    }
}