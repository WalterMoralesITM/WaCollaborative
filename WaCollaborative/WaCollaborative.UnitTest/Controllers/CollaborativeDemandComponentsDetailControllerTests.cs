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
    public class CollaborativeDemandComponentsDetailControllerTests
    {
        private readonly DbContextOptions<DataContext> _options;
        private readonly Mock<IGenericUnitOfWork<CollaborativeDemandComponentsDetail>> _unitOfWorkMock;

        public CollaborativeDemandComponentsDetailControllerTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _unitOfWorkMock = new Mock<IGenericUnitOfWork<CollaborativeDemandComponentsDetail>>();
        }

        [TestMethod]
        public async Task PutAsync_ReturnsNotFoundResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandComponentsDetailController(_unitOfWorkMock.Object, context);
            CollaborativeDemandComponentsDetail demandDetail = BuildCollaborativeDemandComponentsDetail();

            /// Act
            var result = await controller.PutAsync(demandDetail) as OkObjectResult;

            /// Assert
            Assert.IsNull(result);

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task PutAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandComponentsDetailController(_unitOfWorkMock.Object, context);
            CollaborativeDemandComponentsDetail demandDetail = BuildCollaborativeDemandComponentsDetail();
            context.CollaborativeDemandComponentsDetail.Add(demandDetail);
            context.SaveChanges();

            /// Act
            var result = await controller.PutAsync(demandDetail) as OkObjectResult;

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
            var controller = new CollaborativeDemandComponentsDetailController(_unitOfWorkMock.Object, context);
            PaginationDTO paginationDTO = BuildPaginationDTO();

            CollaborativeDemandComponentsDetail demandDetail = BuildCollaborativeDemandComponentsDetail();
            context.CollaborativeDemandComponentsDetail.Add(demandDetail);
            context.SaveChanges();

            /// Act
            var result = await controller.GetAsync(paginationDTO) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetPeriodsAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandComponentsDetailController(_unitOfWorkMock.Object, context);
            int collaborativeDemandId = 1;
            string userId = "1";
            CollaborativeDemandComponentsDetail demandDetail = BuildCollaborativeDemandComponentsDetail();
            context.CollaborativeDemandComponentsDetail.Add(demandDetail);
            context.SaveChanges();

            /// Act
            var result = await controller.GetPeriodsAsync(collaborativeDemandId, userId) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
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

        private CollaborativeDemandComponentsDetail BuildCollaborativeDemandComponentsDetail()
        {
            CollaborativeDemandComponentsDetail demandComponentsDetail = new CollaborativeDemandComponentsDetail
            {
                Id = 1,
                YearMonth = 1,
                CollaborativeDemand = new CollaborativeDemand
                {
                    Id = 2,
                },
                CollaborativeDemandId = 1,
                Quantity = 1,
                UpdateDate = DateTime.UtcNow,
                User = new User
                {
                    Id = "1",
                    Address = "test",
                    Document = "1111",
                    FirstName = "test",
                    LastName = "test"
                },
                UserId = "1"
            };

            return demandComponentsDetail;
        }
    }
}