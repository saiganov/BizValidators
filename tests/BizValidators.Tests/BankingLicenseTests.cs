using NUnit.Framework;
using Shouldly;

namespace BizValidators.Tests
{
    [TestFixture]
    public class BankingLicenseTests
    {
        private IValidator _bankigLicenseValidator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _bankigLicenseValidator = new BankingLicenseValidator();
        }
        
        [TestCase("")]
        [TestCase(null)]
        [TestCase("    ")]
        public void ValidateAsync_IfPassEmptyLicense_ReturnError(string license)
        {
            var result = _bankigLicenseValidator.ValidateAsync(license).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }

        [TestCase("123")]
        [TestCase("12345")]
        [TestCase("12345678")]
        public void ValidateAsync_IfPassLisenceWithInvalidLength_ReturnError(string license)
        {
            var result = _bankigLicenseValidator.ValidateAsync(license).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }

        [TestCase("12Z4")]
        [TestCase("T234")]
        public void ValidateAsync_IfPassLicenseWith4SymbolsAndContainsNotDigit_ReturnError(string license)
        {
            var result = _bankigLicenseValidator.ValidateAsync(license).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("1234")]
        [TestCase("4321")]
        public void ValidateAsync_IfPassLicenseWith4SymbolsAndContainsAllDigit_ReturnSuccess(string license)
        {
            var result = _bankigLicenseValidator.ValidateAsync(license).GetAwaiter().GetResult();
            
            result.IsSuccess.ShouldBeTrue();
        }
        
        [TestCase("12Z456")]
        [TestCase("T23456")]
        public void ValidateAsync_IfPassLicenseWith6SymbolsAndFirst4ContainsNotDigit_ReturnError(string license)
        {
            var result = _bankigLicenseValidator.ValidateAsync(license).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("1234R6")]
        [TestCase("1234.6")]
        public void ValidateAsync_IfPassLicenseWith6SymbolsAnd5SymbolNotDash_ReturnError(string license)
        {
            var result = _bankigLicenseValidator.ValidateAsync(license).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("1234-м")]
        [TestCase("1234-f")]
        public void ValidateAsync_IfPassLicenseWith6SymbolsAndNotValidEndLetter_ReturnError(string license)
        {
            var result = _bankigLicenseValidator.ValidateAsync(license).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("1234-k")] // latin
        [TestCase("1234-K")]
        [TestCase("1234-к")] // ru
        [TestCase("1234-К")]
        public void ValidateAsync_IfPassLicenseWith6SymbolsAndAllValid_ReturnSuccess(string license)
        {
            var result = _bankigLicenseValidator.ValidateAsync(license).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("12Z4567")]
        [TestCase("T234567")]
        public void ValidateAsync_IfPassLicenseWith7SymbolsAndFirst4ContainsNotDigit_ReturnError(string license)
        {
            var result = _bankigLicenseValidator.ValidateAsync(license).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("1234-RF")]
        [TestCase("1234.ЦК")]
        public void ValidateAsync_IfPassLicenseWith7SymbolsAndEndLettersNotValid_ReturnError(string license)
        {
            var result = _bankigLicenseValidator.ValidateAsync(license).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }
        
        [TestCase("1234-ЦК")]
        [TestCase("3451-ЦК")]
        public void ValidateAsync_IfPassLicenseWith7SymbolsAndAllValid_ReturnError(string license)
        {
            var result = _bankigLicenseValidator.ValidateAsync(license).GetAwaiter().GetResult();
            
            result.IsFailure.ShouldBeTrue();
        }
    }
}