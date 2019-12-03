using System.Linq;
using System.Threading.Tasks;
using Core.Utils;
using CSharpFunctionalExtensions;
using StringUsefullExtensions;

namespace BizValidators
{
    public class OkpoBusinessmanValidator : IValidator
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

            var lenghtCheck = LengthCheck(value);
            var digitCheck = DigitCheck(value);

            var basicCheck = Result.Combine(lenghtCheck, digitCheck);
            if (basicCheck.IsFailure)
            {
                return Task.FromResult<Result>(basicCheck);
            }

            var controlSumCheck = ControlSumCheck(value);
            return Task.FromResult<Result>(Result.Combine(basicCheck, controlSumCheck));
        }

        private Result LengthCheck(string value)
        {
            if (value.Length == 10)
            {
                return Result.Ok();
            }
            
            return Result.Fail("Okpo must be 10 symbols");
        }

        private Result DigitCheck(string value)
        {
            if (value.AllDigits())
            {
                return Result.Ok();
            }
            
            return Result.Fail("Okpo must have only digits");
        }

        private Result ControlSumCheck(string value)
        {
            var calculation = value.Take(9).Select(
                                      (c, i) => (i + 1) * c.GetIntFromDigitChar())
                                  .Sum() % 11;
            if (calculation != 10)
            {
                if (calculation == value[9].GetIntFromDigitChar())
                {
                    return Result.Ok();
                }
            }
            else
            {
                calculation = value.Take(9).Select(
                                      (c, i) => (i + 3) * c.GetIntFromDigitChar())
                                  .Sum() % 11;
                if ((calculation == 10 ? 0 : calculation) == value[9].GetIntFromDigitChar())
                {
                    return Result.Ok();
                }
            }
            
            return Result.Fail("Okpo check sum invalid");
        }
    }
}