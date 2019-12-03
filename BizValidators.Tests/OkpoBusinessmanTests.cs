using BizValidators;
using Core;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    public class OkpoBusinessmanTests
    {
        private IValidator _okpoValidator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _okpoValidator = new OkpoBusinessmanValidator();
        }
        
        [TestCase("")]
        [TestCase(null)]
        [TestCase("    ")]
        public void ValidateAsync_IfPassEmptyValue_ReturnError(string value)
        {
            var result = _okpoValidator.ValidateAsync(value).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("123456789")]
        [TestCase("01234569871")]
        public void ValidateAsync_IfPassNot10Symbols_ReturnError(string value)
        {
            var result = _okpoValidator.ValidateAsync(value).GetAwaiter().GetResult();

            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("1234P567891")]
        [TestCase("01!3456987")]
        public void ValidateAsync_IfPassNotDigit_ReturnFalse(string value)
        {
            var result = _okpoValidator.ValidateAsync(value).GetAwaiter().GetResult();

            result.IsFailure.ShouldBeTrue();
        }
        
        [Ignore("Cant find valid OKPO")]
        public void ValidateAsync_IfPassValidValidValue_ReturnSuccess(string value)
        {
            var result = _okpoValidator.ValidateAsync(value).GetAwaiter().GetResult();

            result.IsSuccess.ShouldBeTrue();
        }
    }
}