using BizValidators;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    [TestFixture]
    public class KppTests
    {
        private IValidator _kppValidator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _kppValidator = new KppValidator();
        }
        
        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        public void ValidateAsync_IfPassEmptyKpp_ReturnError(string value)
        {
            var result = _kppValidator.ValidateAsync(value).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("123456789")]
        [TestCase("1234ZD789")]
        [TestCase("123401789")]
        [TestCase("123492789")]
        [TestCase("1234TK789")]
        public void ValidateAsync_IfPassValidKpp_ReturnSuccess(string value)
        {
            var result = _kppValidator.ValidateAsync(value).GetAwaiter().GetResult();

            result.IsSuccess.ShouldBeTrue();
        }
        
        [TestCase("12345D789")]
        [TestCase("1234ZD78")]
        [TestCase("DDDDDDDDD")]
        [TestCase("1234567890")]
        public void ValidateAsync_IfPassInvalidKpp_ReturnError(string value)
        {
            var result = _kppValidator.ValidateAsync(value).GetAwaiter().GetResult();

            result.IsFailure.ShouldBeTrue();
        }
    }
}