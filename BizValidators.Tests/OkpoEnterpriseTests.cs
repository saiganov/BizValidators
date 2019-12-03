using BizValidators;
using Core;
using NUnit.Framework;

namespace Tests
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