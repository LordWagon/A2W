using LoginManager;
using LoginManager.Shared;
using Moq;
using NUnit.Framework;
using static SQLite.SQLite3;

namespace NUnit_Age2Word.CoreFunctionTests
{
    public class CryptoTests
    {
        [Test]
        public void TestInitial()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void TestConnectionToProject()
        {
            int resultFromProject = Crypto.TestOfTest();
            Assert.AreEqual(42, resultFromProject);
        }

        [Test]
        public void TestCorrectEncryptAndBackDecrypt()
        {
            string before = "Test 42";
            string crypted = Crypto.Encrypt(before);
            Console.WriteLine("crypted:");
            Console.WriteLine(crypted);
            string after = Crypto.Decrypt(crypted);

            Assert.AreEqual(before, after);
            Assert.That(after, Is.EqualTo(before));
        }

        [Test]
        public void TestEncryptReturnNotNull()
        {
            string result = Crypto.Encrypt("Test 42");
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestEncryptReturnNotEmpty()
        {
            string result = Crypto.Encrypt("Test 42");
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void TestNumberOfKeyBytesIsNotNegative()
        {
            int length = Crypto.NumberOfKeyBytes();
            Assert.GreaterOrEqual(length, 0);
        }

        [Test]
        public void TestNumberOfKeyBytesArePositive()
        {
            int length = Crypto.NumberOfKeyBytes();
            Assert.Greater(length, 0);
        }

        [Test]
        public void TestNumberOfKeyBytes()
        {
            int length = Crypto.NumberOfKeyBytes();
            Assert.AreEqual(16, length);
        }

        [Test]
        public void TestNumberOfIVBytesIsNotNegative()
        {
            int length = Crypto.NumberOfIVBytes();
            Assert.GreaterOrEqual(length, 0);
        }

        [Test]
        public void TestNumberOfIVBytesArePositive()
        {
            int length = Crypto.NumberOfIVBytes();
            Assert.Greater(length, 0);
        }

        [Test]
        public void TestNumberOfIVBytes()
        {
            int length = Crypto.NumberOfIVBytes();
            Assert.AreEqual(16, length);
        }
    }
}