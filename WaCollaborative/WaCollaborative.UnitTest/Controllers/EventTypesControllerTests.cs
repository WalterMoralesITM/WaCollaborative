using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WaCollaborative.Backend.Controllers;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.UnitTest.Controllers
{
    [TestClass]
    public class EventTypesControllerTests
    {
        private readonly DbContextOptions<DataContext> _options;
        private readonly Mock<IGenericUnitOfWork<EventType>> _unitOfWorkMock;

        public EventTypesControllerTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString())
             .Options;
            _unitOfWorkMock = new Mock<IGenericUnitOfWork<EventType>>();
        }

        [TestMethod]
        public async Task GetComboAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new EventTypesController(_unitOfWorkMock.Object, context);

            /// Act
            var result = await controller.GetComboAsync() as OkObjectResult;

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
            var controller = new EventTypesController(_unitOfWorkMock.Object, context);
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
            var controller = new EventTypesController(_unitOfWorkMock.Object, context);
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
        public async Task GetAsyncWithId_NotFound_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            context.EventTypes.Add(new EventType { Id = 1, Name = "Test" });
            context.SaveChanges();

            var controller = new EventTypesController(_unitOfWorkMock.Object, context);
            int id = 2;

            /// Act
            var result = await controller.GetAsync(id) as OkObjectResult;

            /// Assert
            Assert.IsNull(result);

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetAsyncWithId_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            context.EventTypes.Add(new EventType { Id = 1, Name = "Test" });
            context.SaveChanges();

            var controller = new EventTypesController(_unitOfWorkMock.Object, context);
            int id = 1;

            /// Act
            var result = await controller.GetAsync(id) as OkObjectResult;
            EventType resultEventType = (EventType)result!.Value!;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(resultEventType.Name, "Test");

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }
    }
}