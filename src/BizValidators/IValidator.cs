using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace BizValidators
{
    public interface IValidator
    {
        Result Validate(string value);
        Task<Result> ValidateAsync(string value);
    }
}