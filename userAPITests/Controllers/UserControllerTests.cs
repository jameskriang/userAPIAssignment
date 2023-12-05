namespace userAPITests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using userAPI.Controller;
    using userAPI.Models;
    using userAPI.Services;

    [TestClass]
    public class UserControllerTests
    {
        private UserController _testClass;
        private ILogger<UserController> _logger;
        private IUserService _userService;

        [TestInitialize]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<UserController>>();
            _userService = Substitute.For<IUserService>();
            _testClass = new UserController(_logger, _userService);
        }

        [TestMethod]
        public void CanConstruct()
        {
            // Act
            var instance = new UserController(_logger, _userService);

            // Assert
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public async Task CanCallGetAll()
        {
            _userService.GetAll().Returns(new List<User>() { new User { Id = 1, Name = "Alice", Email = "alice@email.com" } });
            // Act
            var result = await _testClass.GetAll();

            // Assert
            Assert.IsTrue(result.Value.Count > 0);
        }

        [TestMethod]
        public async Task CanCallCreate()
        {
            // Arrange
            var user = new User
            {
                Id = 3,
                Name = "TestName",
                Email = "TestEmail"
            };
            _userService.Add(user).Returns(Task.CompletedTask);

            // Act
            var result = await _testClass.Create(user) as CreatedAtActionResult;
            var userResult = result.Value as User;
            
            // Assert
            Assert.AreEqual(result.StatusCode, 201);
            Assert.AreEqual(userResult, user);
        }

        [TestMethod]
        public async Task CannotCallCreateWithNullUser()
        {
            await Assert.ThrowsExceptionAsync<NullReferenceException>(() => _testClass.Create(null));
        }

        [TestMethod]
        public async Task CanCallUpdate()
        {
            // Arrange
            var id = 2;
            var user = new User
            {
                Id = 2,
                Name = "NewTestName",
                Email = "NewTestEmail"
            };
            _userService.Get(2).Returns(new User { Id = 1, Name = "Alice", Email = "alice@email.com" });
            _userService.Update(user).Returns(Task.CompletedTask);

            // Act
            var result = await _testClass.Update(id, user) as OkResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public async Task CannotCallUpdateWithNullUser()
        {
            await Assert.ThrowsExceptionAsync<NullReferenceException>(() => _testClass.Update(17201661, null));
        }

        [TestMethod]
        public async Task CanCallSearch()
        {
            // Arrange
            var text = "bob";
            _userService.Search(text).Returns(new List<User>() { new User { Id = 1, Name = "Alice", Email = "alice@email.com" } });

            // Act
            var result = await _testClass.Search(text);

            // Assert
            Assert.IsTrue(result.Value.Count > 0);
        }

        [TestMethod]
        public async Task CanCallSecret()
        {
            // Arrange

            // Act
            var result = await _testClass.Secret() as OkResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
        }

    }
}