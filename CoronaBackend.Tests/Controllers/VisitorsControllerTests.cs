using CoronaBackend.Data;
using CoronaBackend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoronaBackend.Controllers.Tests
{
    public class VisitorsControllerTests
    {
        private VisitorsController _visitorController;
        private AppDbContext _contextDb;
        private ILogger<VisitorsController> _logger;

        public VisitorsControllerTests()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<VisitorsController>();

            CreateDataBase();
            SeedDataBase();

            _visitorController = new VisitorsController(_contextDb, _logger);
        }

        [Fact()]
        public async void GetTest()
        {
            var result = await _visitorController.Get();

            Assert.IsType<ActionResult<IEnumerable<Visitor>>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result); 
            var returnValue = Assert.IsType<List<Visitor>>(okObjectResult.Value);

            Assert.Equal(1, returnValue.Count);
        }

        [Fact()]
        public async void GetByIdTest()
        {
            var result = await _visitorController.Get(1);
            Assert.IsType<ActionResult<Visitor>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var visitor = Assert.IsType<Visitor>(okObjectResult.Value);

            Assert.Equal("6395E775-D320-4F38-B6A3-638CAD332BA8", visitor.QrCodeString);
        }

        [Fact()]
        public async void PostTest()
        {
            string qrCodeString = "983B5D9C-0C05-408C-B67B-D87D26A5EB6D";

            Visitor visitor = new Visitor()
            {
                FirstName = "Jane",
                LastName = "Doe",
                BirthDate = new DateOnly(1987, 4, 30),
                QrCodeString = qrCodeString
            };
            
            var result = await _visitorController.Post(visitor);

            Assert.IsType<ActionResult<Visitor>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var newVisitor = Assert.IsType<Visitor>(okObjectResult.Value);

            Assert.Equal(qrCodeString, newVisitor.QrCodeString);
        }

        public void CreateDataBase()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                       .UseInMemoryDatabase(Guid.NewGuid().ToString())
                       .Options;

            _contextDb = new AppDbContext(dbContextOptions);
            _contextDb.Database.EnsureCreated();
        }

        public void SeedDataBase()
        {
            string qrCodeString = "6395E775-D320-4F38-B6A3-638CAD332BA8";

            Visitor visitor = new Visitor()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    BirthDate = new DateOnly(1986, 5, 31),
                    QrCodeString = qrCodeString
            };

            _contextDb.Visitor.Add(visitor);
            _contextDb.SaveChanges();
        }
    }
}