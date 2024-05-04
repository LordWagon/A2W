using LoginManager;
using LoginManager.Shared;
using Moq;
using NUnit.Framework;
using System;


namespace NUnit_Age2Word.DetailPageTests
{
    public class DetailPageTests
    {
        Mock<INavigationService> mockNavigationService;
        Mock<IDataStorageService> mockDataService;
        DetailViewModel viewModel;
        Record testRecord;

        [SetUp]
        public void Setup()
        {
            mockNavigationService = new Mock<INavigationService>();
            mockDataService = new Mock<IDataStorageService>();

            testRecord = new Record { Id = 1, Name = "Test", Location = "Test Location", Username = "TestUser", Password = "TestPass", Notes = "TestNotes" };
            mockDataService.Setup(x => x.Get(It.IsAny<int>())).Returns(testRecord);

            viewModel = new DetailViewModel(mockNavigationService.Object, mockDataService.Object, 1);
        }

        [TearDown]
        public void TearDown()
        {
            mockNavigationService = null;
            mockDataService = null;
            viewModel = null;
        }

        [Test]
        public void TestInitialValues()
        {
            Assert.AreEqual(testRecord.Name, viewModel.Name);
            Assert.AreEqual(testRecord.Location, viewModel.Location);
            Assert.AreEqual(testRecord.Username, viewModel.Username);
            Assert.AreEqual(testRecord.Password, viewModel.Password);
            Assert.AreEqual(testRecord.Notes, viewModel.Notes);
        }

        [Test]
        public void TestSaveCommand()
        {
            // Act
            viewModel.SaveCommand.Execute(null);

            // Assert
            mockDataService.Verify(data => data.Update(It.IsAny<Record>()), Times.Once);
            mockNavigationService.Verify(nav => nav.NavigateToAsync<MainPageViewModel>(), Times.Once);
        }

        [Test]
        public void TestNavigateBackCommand()
        {
            // Act
            viewModel.NavigateBackCommand.Execute(null);

            // Assert
            mockNavigationService.Verify(nav => nav.NavigateToAsync<MainPageViewModel>(), Times.Once);
        }
    }
}
