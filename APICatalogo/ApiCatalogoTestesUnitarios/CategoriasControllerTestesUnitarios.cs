
using APICatalogo.Data;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repository;
using APICatalogo.Repository.interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoTestesUnitarios
{
    public class CategoriasControllerTestesUnitarios
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;        

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=localhost;DataBase=APICatalogoDB;Uid=root; Pwd=UmaSenh@Aki";

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

            _uof = new UnitOfWork(context);
        }
    }
}
