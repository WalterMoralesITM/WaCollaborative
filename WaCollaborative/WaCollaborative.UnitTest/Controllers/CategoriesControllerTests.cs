#region Using

using Microsoft.EntityFrameworkCore;
using Moq;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.Entities;

#endregion Using

namespace WaCollaborative.UnitTest.Controllers
{
    /// <summary>
    /// The class CategoriesControllerTests
    /// </summary>

    [TestClass]
    public class CategoriesControllerTests
    {

        #region Attributes

        private readonly DbContextOptions<DataContext> _options;
        private readonly Mock<IGenericUnitOfWork<Category>> _unitOfWorkMock;

        #endregion Attributes

        #region Constructor

        public CategoriesControllerTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _unitOfWorkMock = new Mock<IGenericUnitOfWork<Category>>();
        }

        #endregion Constructor

        #region Methods

        #endregion Methods

    }
}