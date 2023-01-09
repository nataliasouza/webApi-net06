using APICatalogo.Data;
using APICatalogo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoTestesUnitarios
{
    public class DBUnitTestsMockInitializer
    {
        public DBUnitTestsMockInitializer()
        {}

        public void Seed(AppDbContext context)
        {
            context.Categorias.Add(
                new Categoria { 
                    CategoriaId = 1,
                    Nome = "Bebidas",
                    ImagemUrl = "bebidas.png"
                });

            context.Categorias.Add(
                new Categoria
                {
                    CategoriaId = 2,
                    Nome = "Pizzas",
                    ImagemUrl = "pizzas.png"
                });

            context.Categorias.Add(
                new Categoria
                {
                    CategoriaId = 3,
                    Nome = "Salgados",
                    ImagemUrl = "salgados.png"
                });

            context.Categorias.Add(
                new Categoria
                {
                    CategoriaId = 4,
                    Nome = "Bolos",
                    ImagemUrl = "bolos.png"
                });

            context.Categorias.Add(
                new Categoria
                {
                    CategoriaId = 5,
                    Nome = "Chocolates",
                    ImagemUrl = "chocolates.png"
                });

            context.SaveChanges();
        }
    }
}
