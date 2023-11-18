#region Using

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WaCollaborative.Backend.Controllers;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;

#endregion Using

namespace WaCollaborative.UnitTest.Controllers
{
    /// <summary>
    /// The class StatesControllerTests
    /// </summary>

    [TestClass]
    public class StatesControllerTests
    {

        #region Attributes

        private readonly DbContextOptions<DataContext> _options;
        private readonly Mock<IGenericUnitOfWork<State>> _unitOfWorkMock;

        #endregion Attributes

        #region Constructor

        public StatesControllerTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _unitOfWorkMock = new Mock<IGenericUnitOfWork<State>>();
        }

        #endregion Constructor

        #region Methods

        [TestMethod]
        public async Task GetComboAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new StatesController(_unitOfWorkMock.Object, context);
            var countryId = 1;

            /// Act
            var result = await controller.GetComboAsync(countryId) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new StatesController(_unitOfWorkMock.Object, context);
            var pagination = new PaginationDTO { Id = 1, Filter = "Some" };

            /// Act
            var result = await controller.GetAsync(pagination) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new StatesController(_unitOfWorkMock.Object, context);
            var pagination = new PaginationDTO { Id = 1, Filter = "Some" };

            /// Act
            var result = await controller.GetPagesAsync(pagination) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetAsync_ReturnsNotFoundWhenStateNotFound()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new StatesController(_unitOfWorkMock.Object, context);

            /// Act
            var result = await controller.GetAsync(1) as NotFoundResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetAsync_ReturnsOkWhenStateFound()
        {
            /// Arrange
            using var context = new DataContext(_options);
            List<City> cities = new List<City>();
            cities.Add(new City { Id = 1, Name = "test",StateId = 1});
            var state = new State { Id = 1, Name = "test", Cities = cities };
            context.States.Add(state);
            context.SaveChanges();

            var controller = new StatesController(_unitOfWorkMock.Object, context);

            /// Act
            var result = await controller.GetAsync(state.Id) as OkObjectResult;
            State resultState = (State)result!.Value!;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(resultState.Name, "test");

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        #endregion Methods

    }
}