using APICatalogo.Controllers;
using APICatalogo.Data;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repository;
using APICatalogo.Repository.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace ApiCatalogoTestesUnitarios
{
    public class CategoriasControllerTestesUnitarios
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriasController> _logger;       

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=localhost;DataBase=APICatalogoDB;Uid=root; Pwd=UmaSenha";       

        static CategoriasControllerTestesUnitarios()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;           
        }

        public CategoriasControllerTestesUnitarios()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            _mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            //DBUnitTestsMockInitializer db = new DBUnitTestsMockInitializer();
            //db.Seed(context);

            _uof = new UnitOfWork(context);

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<CategoriasController>();

            _logger = logger;
        }

        [Fact]
        public async Task GetCategoriaById_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(_uof, _mapper, _logger);
            var catId = 2;

            //Act  
            var data = await controller.GetCategoria(catId);

            //Assert  
            Assert.IsType<CategoriaDTO>(data.Value);
        }

        [Fact]
        public async Task GetCategoriaById_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new CategoriasController(_uof, _mapper, _logger);
            var catId = 9999;

            //Act  
            var data = await controller.GetCategoria(catId);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(data.Result);
        }
    }
}
