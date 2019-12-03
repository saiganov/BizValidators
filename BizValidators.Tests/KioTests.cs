using BizValidators;
using Core;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    public class KioTests
    {
        private IValidator _kioValidatorRu;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _kioValidatorRu = new KioValidator();
        }
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        public void ValidateAsync_IfPassEmptyOrWhitespaceKio_ReturnError(string value)
        {
            var result = _kioValidatorRu.ValidateAsync(value).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }

        [TestCase("1234r")]
        [TestCase("w1234")]
        [TestCase("asf")]
        public void ValidateAsync_IfPassNotOnlyDigitsKio_ReturnError(string value)
        {
            var result = _kioValidatorRu.ValidateAsync(value).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }

        [TestCase("123456")]
        [TestCase("1234")]
        public void ValidateAsync_IfPassNot5SymbolsKio_ReturnError(string value)
        {
            var result = _kioValidatorRu.ValidateAsync(value).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }

        [TestCase("12345")]
        [TestCase("54321")]
        public void ValidateAsync_IfPassValidKio_ReturnError(string value)
        {
            var result = _kioValidatorRu.ValidateAsync(value).GetAwaiter().GetResult();
            
            result.IsSuccess.ShouldBeTrue();
        }
    }
}