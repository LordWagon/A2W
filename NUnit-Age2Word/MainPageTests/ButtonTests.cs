using LoginManager;
using LoginManager.Shared;
using Moq;

namespace NUnit_Age2Word.MainPageTests
{
    public class ButtonTests
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
        public void TestButtonAddEnabled()
        {
            // Act
            bool result = viewModel.AddCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(result);
        }
        
        [Test]
        public void TestButtonAddClicked()
        {
            // Act
            viewModel.AddCommand.Execute(null);

            // Assert
            mockService.Verify(nav => nav.NavigateToAsync<NewRecordViewModel>(), Times.Once);
        }

        [Test]
        public void TestButtonSettingEnabled()
        {
            // Act
            bool result = viewModel.SettingCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(result);
        }
        
        [Test]
        public void TestButtonSettingClicked()
        {
            // Act
            viewModel.SettingCommand.Execute(null);

            // Assert
            mockService.Verify(nav => nav.NavigateToAsync<SettingViewModel>(), Times.Once);
        }
    }
}