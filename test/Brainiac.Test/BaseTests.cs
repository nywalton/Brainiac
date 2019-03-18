using NUnit.Framework;

namespace Brainiac.Test
{
    /// <summary>
    /// BaseTests
    /// </summary>
    public class BaseTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Ensure UnitTests can run. This test should always pass to ensure the framework is set up to run unit tests correctly.
        /// </summary>
        [Test]
        public void BaseTest()
        {
            Assert.Pass();
        }
    }
}