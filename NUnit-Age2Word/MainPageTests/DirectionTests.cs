using LoginManager;
using LoginManager.Shared;
using Moq;
using NUnit.Framework;
using static SQLite.SQLite3;

namespace NUnit_Age2Word.MainPageTests
{
    public class DirectionTests
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
        public void TestInitDirection0()
        {
            Assert.AreEqual("UP", viewModel.Direction0);
        }

        [Test]
        public void TestInitDirection1()
        {
            Assert.AreEqual("UP", viewModel.Direction1);
        }

        [Test]
        public void TestInitDirection2()
        {
            Assert.AreEqual("UP", viewModel.Direction2);
        }

        [Test]
        public void TestInitTapDirection0()
        {
            Assert.AreEqual("UP", viewModel.Direction0);
            viewModel.LabelTappedCommand.Execute("0");

            Assert.AreEqual("DOWN", viewModel.Direction0);
            viewModel.LabelTappedCommand.Execute("0");

            Assert.AreEqual("UP", viewModel.Direction0);
        }

        [Test]
        public void TestInitTapDirection1()
        {
            Assert.AreEqual("UP", viewModel.Direction1);
            viewModel.LabelTappedCommand.Execute("1");

            Assert.AreEqual("DOWN", viewModel.Direction1);
            viewModel.LabelTappedCommand.Execute("1");

            Assert.AreEqual("UP", viewModel.Direction1);
        }

        [Test]
        public void TestInitTapDirection2()
        {
            Assert.AreEqual("UP", viewModel.Direction2);
            viewModel.LabelTappedCommand.Execute("2");

            Assert.AreEqual("DOWN", viewModel.Direction2);
            viewModel.LabelTappedCommand.Execute("2");

            Assert.AreEqual("UP", viewModel.Direction2);
        }

        [Test]
        public void TestDirectionConsistence()
        {
            string originalDirection0 = viewModel.Direction0;
            string originalDirection1 = viewModel.Direction1;
            string originalDirection2 = viewModel.Direction2;

            viewModel.LabelTappedCommand.Execute("0");
            string direction0 = viewModel.Direction0;
            string direction1 = viewModel.Direction1;
            string direction2 = viewModel.Direction2;

            Assert.AreNotEqual(originalDirection0, direction0);
            originalDirection0 = direction0;
            Assert.AreEqual(originalDirection1, direction1);
            Assert.AreEqual(originalDirection2, direction2);

            viewModel.LabelTappedCommand.Execute("1");
            direction0 = viewModel.Direction0;
            direction1 = viewModel.Direction1;
            direction2 = viewModel.Direction2;
            Assert.AreNotEqual(originalDirection1, direction1);
            originalDirection1 = direction1;
            Assert.AreEqual(originalDirection0, direction0);
            Assert.AreEqual(originalDirection2, direction2);

            viewModel.LabelTappedCommand.Execute("2");
            direction0 = viewModel.Direction0;
            direction1 = viewModel.Direction1;
            direction2 = viewModel.Direction2;
            Assert.AreNotEqual(originalDirection2, direction2);
            originalDirection2 = direction2;
            Assert.AreEqual(originalDirection0, direction0);
            Assert.AreEqual(originalDirection1, direction1);
        }
    }
}