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

namespace WaCollaborative.UnitTest.Controllers
{
    [TestClass]
    public class CollaborativeDemandControllerTests
    {
        private Mock<IUserHelper> _mockUserHelper = null!;
        private Mock<IMailHelper> _mockMailHelper = null!;
        private readonly DbContextOptions<DataContext> _options;
        private readonly Mock<IGenericUnitOfWork<CollaborativeDemand>> _unitOfWorkMock;
        
    }
}
