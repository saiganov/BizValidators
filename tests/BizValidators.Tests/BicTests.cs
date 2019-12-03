using NUnit.Framework;
using Shouldly;

namespace BizValidators.Tests
{
    public class BicTests
    {
        private IValidator _bicValidator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _bicValidator = new BicValidator();
        }
        
        [TestCase("04388210")]
        [TestCase("0438821062")]
        public void ValidateAsync_IfPassbicWithNot9Chars_ReturnError(string value)
        {
            var result = _bicValidator.ValidateAsync(value).GetAwaiter().GetResult();

            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("0438D2106")]
        [TestCase("042T16881")]
        public void ValidateAsync_IfBicContainsNotNumbers_ReturnError(string value)
        {
            var result = _bicValidator.ValidateAsync(value).GetAwaiter().GetResult();

            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("013882106")]
        [TestCase("142116881")]
        public void ValidateAsync_IfBicNotStartWith04_ReturnError(string value)
        {
            var result = _bicValidator.ValidateAsync(value).GetAwaiter().GetResult();

            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("047072017")]
        [TestCase("047188441")]
        public void ValidateAsync_IfPassValidBic_ReturnSuccess(string value)
        {
            var result = _bicValidator.ValidateAsync(value).GetAwaiter().GetResult();

            result.IsFailure.ShouldBeFalse();
        }
    }
}