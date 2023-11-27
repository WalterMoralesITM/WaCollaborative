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
    public class CollaborativeDemandUsersControllerTest
    {
        private readonly DbContextOptions<DataContext> _options;
        private readonly Mock<IGenericUnitOfWork<CollaborativeDemandUsers>> _unitOfWorkMock;

        public CollaborativeDemandUsersControllerTest()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _unitOfWorkMock = new Mock<IGenericUnitOfWork<CollaborativeDemandUsers>>();
        }

        [TestMethod]
        public async Task GetProductsAsyncc_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandUsersController(_unitOfWorkMock.Object, context);

            PaginationDTO paginationDTO = BuildPaginationDTO();

            /// Act
            var result = await controller.GetProductsAsync(paginationDTO) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task GetShippingPointsAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandUsersController(_unitOfWorkMock.Object, context);

            PaginationDTO paginationDTO = BuildPaginationDTO();

            /// Act
            var result = await controller.GetShippingPointsAsync(paginationDTO) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task GetAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandUsersController(_unitOfWorkMock.Object, context);

            PaginationDTO paginationDTO = BuildPaginationDTO();

            /// Act
            var result = await controller.GetAsync(paginationDTO) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task GetAsync_WithId_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandUsersController(_unitOfWorkMock.Object, context);

            /// Act
            var result = await controller.GetAsync(1) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task GetDetailAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandUsersController(_unitOfWorkMock.Object, context);

            PaginationDTO paginationDTO = BuildPaginationDTO();

            /// Act
            var result = await controller.GetDetailAsync(paginationDTO, "1") as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandUsersController(_unitOfWorkMock.Object, context);

            PaginationDTO paginationDTO = BuildPaginationDTO();

            /// Act
            var result = await controller.GetPagesAsync(paginationDTO) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        private PaginationDTO BuildPaginationDTO()
        {
            PaginationDTO paginationDTO = new PaginationDTO
            {
                Filter = "1",
                Id = 1,
                Page = 1,
                RecordsNumber = 1
            };

            return paginationDTO;
        }
    }
}