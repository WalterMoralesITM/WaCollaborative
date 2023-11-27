using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
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
using WaCollaborative.Backend.Helpers.Interfaces;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.DTOs;

namespace WaCollaborative.UnitTest.Controllers
{
    [TestClass]
    public class CollaborativeDemandControllerTests
    {
        #region Attributes

        private Mock<IUserHelper> _mockUserHelper = null!;
        private Mock<IMailHelper> _mockMailHelper = null!;
        private CollaborativeDemandController _controller = null!;
        private DataContext _context = null!;
        private  Mock<IGenericUnitOfWork<CollaborativeDemand>> _unitOfWorkMock = null!;

        #endregion
        #region Constructor

        #endregion

        [TestInitialize]
        public void Setup()
        {
            _mockUserHelper = new Mock<IUserHelper>();
            _mockMailHelper = new Mock<IMailHelper>();
            _unitOfWorkMock = new Mock<IGenericUnitOfWork<CollaborativeDemand>>();
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new DataContext(options);

            _controller = new CollaborativeDemandController(_unitOfWorkMock.Object, _context, _mockUserHelper.Object, _mockMailHelper.Object)
            {
                
            };
        }

        #region Methods        
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsOkResult()
        {
            /// Arrange            

            ///// Act
            var result = await _controller.GetAllAsync() as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var resultList = result.Value as List<CollaborativeDemandDTO>;
            Assert.IsNotNull(resultList);
            // Agrega más aserciones según la estructura esperada de CollaborativeDemandDTO

            // Ejemplo: Verificar que hay al menos un elemento en la lista
            Assert.IsTrue(resultList.Count > 0);
        }

        #endregion
    }
}
