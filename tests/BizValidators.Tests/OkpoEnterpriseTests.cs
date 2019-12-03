using NUnit.Framework;

namespace BizValidators.Tests
{
    [TestFixture]
    public class OkpoEnterpriseTests
    {
        private IValidator _okpoValidator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _okpoValidator = new OkpoEnterpriseValidator();
        }
        
        [TestCase("")]
        [TestCase(null)]
        [TestCase("    ")]
        public void ValidateAsync_IfPassEmptyValue_ThrowException(string value)
        {
            
        }
    }
}