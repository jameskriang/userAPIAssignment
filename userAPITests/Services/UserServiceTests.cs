namespace userAPITests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using userAPI.Models;
    using userAPI.Services;

    [TestClass]
    public class UserServiceTests
    {
        private UserService _testClass;

        [TestInitialize]
        public void SetUp()
        {
            _testClass = new UserService();
        }

        [TestMethod]
        public async Task CanCallGetAll()
        {
            // Act
            var result = await _testClass.GetAll();

            // Assert
            Assert.IsTrue(result.Count >= 1);
        }

        [TestMethod]
        public async Task CanCallAdd()
        {
            // Arrange
            var user = new User
            {
                Name = "TestName",
                Email = "TestEmail"
            };

            // Act
            await _testClass.Add(user);

            // Assert
            var result = await _testClass.GetAll();
            Assert.IsTrue(result.Any(i => i.Name.Equals(user.Name)));
        }

        [TestMethod]
        public async Task CannotCallAddWithNullUser()
        {
            await Assert.ThrowsExceptionAsync<NullReferenceException>(() => _testClass.Add(null));
        }

        [TestMethod]
        public async Task CanCallGet()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _testClass.Get(id);

            // Assert
            Assert.AreEqual(result.Name, "Alice");
        }

        [TestMethod]
        public async Task CanCallUpdate()
        {
            // Arrange
            var id = 2;
            var newName = "NewTestName";
            var newTest = "NewTestEmail";
            var user = new User
            {
                Id = id,
                Name = newName,
                Email = newTest
            };

            // Act
            await _testClass.Update(user);

            // Assert
            var result = await _testClass.Get(id);
            Assert.AreEqual(result.Name, newName);
            Assert.AreEqual(result.Email, newTest);
        }

        [TestMethod]
        public async Task CannotCallUpdateWithNullUser()
        {
            await Assert.ThrowsExceptionAsync<NullReferenceException>(() => _testClass.Update(null));
        }

        [TestMethod]
        public async Task CanCallSearch()
        {
            // Arrange
            var text = "lice";

            // Act
            var result = await _testClass.Search(text);

            // Assert
            Assert.IsTrue(result.Count >= 1);
        }

        public async Task CannotCallSearchWithInvalidText(string value)
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _testClass.Search(null));
        }
    }
}