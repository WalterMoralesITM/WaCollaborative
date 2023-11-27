using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaCollaborative.Backend.Controllers;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.UnitTest.Controllers
{
    [TestClass]
    public class CollaborativeDemandsDemoControllerTests
    {
        private readonly DbContextOptions<DataContext> _options;
        private readonly Mock<IGenericUnitOfWork<CollaborativeDemandDemo>> _unitOfWorkMock;

        public CollaborativeDemandsDemoControllerTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _unitOfWorkMock = new Mock<IGenericUnitOfWork<CollaborativeDemandDemo>>();
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsNotFoundResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandsDemoController(_unitOfWorkMock.Object, context);
            PaginationDTO paginationDTO = BuildPaginationDTO();

            CollaborativeDemandDemo demo = new CollaborativeDemandDemo
            {
                CityName = "test",
                CustomerName = "test",
                Id = 1,
                ProductName = "test",
                Quantity = 1,
                YearMonth = 202310
            };

            context.CollaborativeDemandDemo.Add(demo);
            context.SaveChanges();
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
