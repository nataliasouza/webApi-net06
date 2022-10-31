# Web API ASP .NET Core Essencial (.NET 6) - Macoratti - => Em Desenvolvimento <=

### Recursos da ASP .NET Core WEB API utilizados : Roteamento, Padrões de rotas, Tipos de retorno, Model Binding, Data Annotations, Configuração, Filtros, Tratamento de Erros, Logging.

> Outros conceitos: Paginação, Programação Assíncrona, Segurança (Identity - JWT), Documentação com Swagger e Testes Unitários.

### *Criação Web API no VS 2022*

### *Version = "v1"*

1.	Criar projeto no VS 2022 Community – ApiCatalogo.
2.	Criar o projeto com opção para habilitar a Open API e usar Controllers.
3.	Criar o modelo de entidades – Produto e Categoria.
4.	Configurar o projeto para usar o EF Core e incluir referências ao EF Core.
5.	Definir o banco de dados usado – MySql e MySQL Workbench.
6.	Definir a classe de contexto do EF Core – AppDbContext
7.	Definir o mapeamento de entidades para as tabelas – DbSet<T>
8.	Registrar o contexto como um serviço – Program
9.	Definir a string de conexão no arquivo appsettings.json
10.	Definir o provedor do banco de dados (Pomelo) e obter a string de conexão.
11.	Aplicar o Migrations e criar o banco de dados e as tabelas.
12.	Criar os controladores : ProdutosController e CategoriaController.
13.	Definir os endpoints ou métodos Actions para realizar as operações CRUD.
14. Utilizar o AutoMapper e criar os DTOs das entidades – ProdutoDTO e CategoriaDTO.
15. Definir os Data Annotations nos atributos dos DTOS criados. 
16. Introduzir o Padrão Repository.
17. Introduzir o Padão UnitOfWork.   
18. Paginação - Get/categorias e Get/produtos.
19. Criação dos Filters - LogginsFilter.
20. Tratamentos de Erros.
20. Criação e Customização dos Loggings.
21. Registro dos Loggings em .txt
22. Programação Assíncrona - Repositório, Paginação e Controladores.
23. Segurança - Autenticação e Autorização - Identity/JWT.
24. Registro, Login e Token - AutorizaController.
25. Configuração do Swagger para utilizar o token JWT.
26. Implementação CORS.  

### *Version = "v2"*
##### Branch = apiCatalogoV2

*O projeto ainda está em desenvolvimento e as próximas atualizações serão voltadas nas seguintes tarefas:*

- [x] Criar o Versionamento da API.
- [ ] Incluir os Teste Unitários.
- [ ] Consumir uma API.

#
### Documentação - Version = "v1"
![image](https://user-images.githubusercontent.com/13735095/199120458-a3f81294-0be6-4680-9c80-827e3d5a4296.png)
![image](https://user-images.githubusercontent.com/13735095/199120534-76a3d776-3cce-4a8e-a1e1-4e0433171572.png)
#
### Documentação - Version = "v2"
![image](https://user-images.githubusercontent.com/13735095/199120719-b0637f04-459c-4b19-88ff-eb241e3d4353.png)



