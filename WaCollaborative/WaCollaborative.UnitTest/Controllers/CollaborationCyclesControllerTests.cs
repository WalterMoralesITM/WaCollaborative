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
    public class CollaborationCyclesControllerTests
    {
        private readonly DbContextOptions<DataContext> _options;
        private readonly Mock<IGenericUnitOfWork<CollaborationCycle>> _unitOfWorkMock;
        public CollaborationCyclesControllerTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _unitOfWorkMock = new Mock<IGenericUnitOfWork<CollaborationCycle>>();
        }

        [TestMethod]
        public async Task GetComboAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborationCyclesController(_unitOfWorkMock.Object, context);

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
            var controller = new CollaborationCyclesController(_unitOfWorkMock.Object, context);            
            /// Act
            var result = await controller.GetAsync() as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }
    }
}
