using LoginManager;
using LoginManager.Shared;

namespace MSTest_Age2Word
{
    [TestClass]
    public class CoreFuctionTests
    {
        [TestMethod]
        public void TestInitial()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestConnectionToProject()
        {
            int resultFromProject = Crypto.TestOfTest();
            Assert.AreEqual(42, resultFromProject);
        }

        [TestMethod]
        public void TestEncryptReturnNotNull()
        {
            string result = Crypto.Encrypt("Test 42");

            Assert.IsNotNull(result);
        }
    }
}