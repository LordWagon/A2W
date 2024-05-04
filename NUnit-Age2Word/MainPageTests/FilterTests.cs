using LoginManager;
using LoginManager.Shared;
using Moq;
using NUnit.Framework;
using static SQLite.SQLite3;

namespace NUnit_Age2Word.MainPageTests
{
    public class FilterTests
    {
        Mock<INavigationService> mockService;
        Mock<IDataStorageService> mockDataService;
        MainPageViewModel viewModel;

        [SetUp]
        public void Setup() 
        {
            mockService = new Mock<INavigationService>();
            mockDataService = new Mock<IDataStorageService>();

            mockDataService.Setup(x => x.GetAll()).Returns(new List<Record>()
            {
                new Record { Id = 0, Name = "ZERO", DateCreated = DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToString()},
                new Record { Id = 1, Name = "ONE", DateCreated = DateTime.Now.Subtract(TimeSpan.FromDays(2)).ToString() },
                new Record { Id = 2, Name = "TWO", DateCreated = DateTime.Now.Subtract(TimeSpan.FromDays(3)).ToString() }
            });
            viewModel = new MainPageViewModel(mockService.Object, mockDataService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            mockService = null;
            mockDataService = null;
            viewModel = null;
        }

        [Test]
        public void TestInitial()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void TestFilterOneValue()
        {
            viewModel.FilterText = "ONE";
            Assert.AreEqual(1, viewModel.Items.Count);
        }

        [Test]
        public void TestNotCaseSensitiveOneValue()
        {
            viewModel.FilterText = "one";
            Assert.AreEqual(1, viewModel.Items.Count);
        }

        [Test]
        public void TestMoreValues()
        {
            viewModel.FilterText = "E";
            Assert.AreEqual(2, viewModel.Items.Count);
        }
        [Test]
        public void TestNotCaseSensitiveMoreValues()
        {
            viewModel.FilterText = "e";
            Assert.AreEqual(2, viewModel.Items.Count);
        }

        [Test]
        public void TestNoneValues()
        {
            viewModel.FilterText = "X";
            Assert.AreEqual(0, viewModel.Items.Count);
        }
        [Test]
        public void TestNotCaseSensitiveNoneValues()
        {
            viewModel.FilterText = "x";
            Assert.AreEqual(0, viewModel.Items.Count);
        }
    }
}