using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WaCollaborative.Backend.Controllers;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Helpers.Interfaces;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.Enums;

namespace WaCollaborative.UnitTest.Controllers
{
    [TestClass]
    public class CollaborativeDemandControllerTests
    {
        private readonly DbContextOptions<DataContext> _options;
        private readonly Mock<IGenericUnitOfWork<CollaborativeDemand>> _unitOfWorkMock;
        private Mock<IUserHelper> _mockUserHelper;
        private Mock<IMailHelper> _mockMailHelper;
        private Mock<IExcelGenerator> _mockExcelGenerator;
        private Mock<MemoryStream> _mockMemoryStream;

        public CollaborativeDemandControllerTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _unitOfWorkMock = new Mock<IGenericUnitOfWork<CollaborativeDemand>>();
            _mockUserHelper = new Mock<IUserHelper>();
            _mockMailHelper = new Mock<IMailHelper>();
            _mockExcelGenerator = new Mock<IExcelGenerator>();
            _mockMemoryStream = new Mock<MemoryStream>();
        }

        [TestMethod]
        public async Task GetAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandController(_unitOfWorkMock.Object, context, _mockUserHelper.Object, _mockMailHelper.Object, _mockExcelGenerator.Object);

            User user = new User
            {
                Id = "1",
                Address = "test",
                Document = "1111",
                FirstName = "test",
                LastName = "test",
                UserType = UserType.Planner
            };
            _mockUserHelper.Setup(x => x.GetUserAsync(It.IsAny<string>()))
               .Returns(Task.FromResult<User>(user));

            /// Act
            var result = await controller.GetAsync() as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task GetAsync_WithCollaborator_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandController(_unitOfWorkMock.Object, context, _mockUserHelper.Object, _mockMailHelper.Object, _mockExcelGenerator.Object);

            List<CollaborationCalendar> collaborationCalendars = new List<CollaborationCalendar>();
            collaborationCalendars.Add(new CollaborationCalendar { Id = 1, EndDate = DateTime.Now });

            User user = new User
            {
                Id = "1",
                Address = "test",
                Document = "1111",
                FirstName = "test",
                LastName = "test",
                UserType = UserType.Collaborator,
                Email = string.Empty,
                InternalRole = new InternalRole { Id = 1, CollaborationCalendars = collaborationCalendars, Name = "test" }
            };

            context.Users.Add(user);
            context.SaveChanges();

            _mockUserHelper.Setup(x => x.GetUserAsync(It.IsAny<string>()))
               .Returns(Task.FromResult<User>(user));

            /// Act
            var result = await controller.GetAsync() as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandController(_unitOfWorkMock.Object, context, _mockUserHelper.Object, _mockMailHelper.Object, _mockExcelGenerator.Object);

            CollaborativeDemand collaborativeDemand = BuildCollaborativeDemand();

            context.CollaborativeDemand.Add(collaborativeDemand);
            context.SaveChanges();

            PaginationDTO paginationDTO = BuildPaginationDTO();

            /// Act
            var result = await controller.GetAsync(paginationDTO) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_WithoutFilter_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandController(_unitOfWorkMock.Object, context, _mockUserHelper.Object, _mockMailHelper.Object, _mockExcelGenerator.Object);

            CollaborativeDemand collaborativeDemand = BuildCollaborativeDemand();

            context.CollaborativeDemand.Add(collaborativeDemand);
            context.SaveChanges();

            PaginationDTO paginationDTO = BuildPaginationDTO_WithoutFilter();

            /// Act
            var result = await controller.GetAsync(paginationDTO) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            /// Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandController(_unitOfWorkMock.Object, context, _mockUserHelper.Object, _mockMailHelper.Object, _mockExcelGenerator.Object);

            User user = new User
            {
                Id = "1",
                Address = "test",
                Document = "1111",
                FirstName = "test",
                LastName = "test",
                UserType = UserType.Planner
            };
            _mockUserHelper.Setup(x => x.GetUserAsync(It.IsAny<string>()))
               .Returns(Task.FromResult<User>(user));

            /// Act
            var result = await controller.GetAllAsync() as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task GetPagesAsync_WithPagination_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandController(_unitOfWorkMock.Object, context, _mockUserHelper.Object, _mockMailHelper.Object, _mockExcelGenerator.Object);

            User user = new User
            {
                Id = "1",
                Address = "test",
                Document = "1111",
                FirstName = "test",
                LastName = "test",
                UserType = UserType.Planner
            };
            _mockUserHelper.Setup(x => x.GetUserAsync(It.IsAny<string>()))
               .Returns(Task.FromResult<User>(user));

            PaginationDTO paginationDTO = BuildPaginationDTO();

            /// Act
            var result = await controller.GetPagesAsync(paginationDTO) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task GetPagesAsync_WithPagination_UserNotFound_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandController(_unitOfWorkMock.Object, context, _mockUserHelper.Object, _mockMailHelper.Object, _mockExcelGenerator.Object);

            PaginationDTO paginationDTO = BuildPaginationDTO();

            /// Act
            var result = await controller.GetPagesAsync(paginationDTO) as OkObjectResult;

            /// Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAsyncExcel_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandController(_unitOfWorkMock.Object, context, _mockUserHelper.Object, _mockMailHelper.Object, _mockExcelGenerator.Object);

            User user = new User
            {
                Id = "1",
                Address = "test",
                Document = "1111",
                FirstName = "test",
                LastName = "test",
                UserType = UserType.Planner
            };
            _mockUserHelper.Setup(x => x.GetUserAsync(It.IsAny<string>()))
               .Returns(Task.FromResult<User>(user));

            /// Act
            var result = await controller.GetAsync(null) as OkObjectResult;

            /// Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task GetAsyncExcel_NotFoundUser_ReturnsOkResult()
        {
            /// Arrange
            using var context = new DataContext(_options);
            var controller = new CollaborativeDemandController(_unitOfWorkMock.Object, context, _mockUserHelper.Object, _mockMailHelper.Object, _mockExcelGenerator.Object);

            _mockUserHelper.Setup(x => x.GetUserAsync(It.IsAny<string>()))
               .Returns(Task.FromResult<User>(null));

            /// Act
            var result = await controller.GetAsync(null) as OkObjectResult;

            /// Assert
            Assert.IsNull(result);
        }

        private CollaborativeDemand BuildCollaborativeDemand()
        {
            List<CollaborativeDemandUsers> users = new List<CollaborativeDemandUsers>();
            users.Add(new CollaborativeDemandUsers { Id = 1, UserId = "1", User = BuildUser() });

            CollaborativeDemand collaborativeDemand = new CollaborativeDemand
            {
                Product = new Product { Id = 1, Name = "Test", Code = "1" },
                ShippingPoint = new ShippingPoint { Id = 1, Name = "Test" },
                CollaborativeDemandUsers = users
            };

            return collaborativeDemand;
        }

        private PaginationDTO BuildPaginationDTO_WithoutFilter()
        {
            PaginationDTO paginationDTO = new PaginationDTO
            {
                Filter = null,
                Id = 1,
                Page = 1,
                RecordsNumber = 1
            };

            return paginationDTO;
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

        private User BuildUser()
        {
            User user = new User
            {
                Id = "1",
                Address = "test",
                Document = "1111",
                FirstName = "test",
                LastName = "test"
            };
            return user;
        }
    }
}