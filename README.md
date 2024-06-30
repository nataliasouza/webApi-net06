# Web API ASP .NET Core Essencial (.NET 6) - Macoratti 
<img src="https://img.shields.io/static/v1?label=MACORATTI&message=UDEMY&color=7159c1&style=for-the-badge"/>
:spiral_calendar: Atualizado em 30 de Junho de 2024

:construction: O projeto está em desenvolvimento :construction:

### Recursos da ASP .NET Core WEB API utilizados : Roteamento, Padrões de rotas, Tipos de retorno, Model Binding, Data Annotations, Configuração, Filtros, Tratamento de Erros, Logging.

> Outros conceitos: Paginação, Programação Assíncrona, Segurança (Identity - JWT), Documentação com Swagger e Testes Unitários.
 
```bash
https://www.udemy.com/course/curso-web-api-asp-net-core-essencial/
```
#
### Criação Web API no VS 2022

### *Version = "v1"*
##### Branch = apiCatalogoV1
<details>
  <summary> <b> Detalhamento das atividades </b> <i>(clique na setinha!)</i> </summary><br>
  
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
</details>

<br>

### *Version = "v2"*
##### Branch = apiCatalogoV2 

<details>
  <summary> <b> Detalhamento das atividades </b> <i>(clique na setinha!)</i> </summary><br>

1.	Criar o Versionamento da API.
2. Criar e Utilizar o Versionamento do Swagger.

:warning: *Esse versionamento foi criado de uma maneira diferente do apresentado no curso.* :warning:

<b>Links utilizados: </b> 
```bash
https://renatogroffe.medium.com/net-5-asp-net-core-swagger-descomplicando-o-versionamento-de-apis-rest-b3641c34203f
```
```bash
https://blog.christian-schou.dk/how-to-use-api-versioning-in-net-core-web-api/
```
</details>
<br>

### *Version = "comentarios"*
##### Branch = comentariosExplicativos

<details>
  <summary> <b> Detalhamento das atividades </b> <i>(clique na setinha!)</i> </summary><br>

1.	Incluir comentários explicando trechos do código. 
2. Incluir no README detalhamento de como executar a aplicação.
3. Incluir no README quais ferramentas foram necessárias.

</details>
<br>

### *Version = "testesUnitarios"*
##### Branch = testesUnitarios

<details>
  <summary> <b> Detalhamento das atividades </b> <i>(clique na setinha!)</i> </summary><br>

1. Incluir os Teste Unitários e utilizar o MOQ. [Em Andamento]

<b>Links utilizados: </b> 
```bash
https://learn.microsoft.com/pt-br/aspnet/core/fundamentals/logging/?view=aspnetcore-6.0
```
```bash
https://stackoverflow.com/questions/43424095/how-to-unit-test-with-ilogger-in-asp-net-core
```
</details>
<br>

### *Version = "jaeger-tracing"*
##### Branch = jaeger-tracing

<details>
  <summary> <b> Detalhamento das atividades </b> <i>(clique na setinha!)</i> </summary><br>

1.	Incluir classes para utilizar o tracing. [concluído]
2. Visualização do Tracing no jaeger. [concluído]

:warning: *No curso não foi demonstrado como utilizar Jaeger* :warning:

<b>Links utilizados: </b> 

Observabilidade em APIs ASP.NET Core com Jaeger - com Henrique Mauri
```bash
https://www.youtube.com/watch?v=pKbhVASHolQ
```
```bash
https://henriquemauri.net/jaeger-e-opentelemetry-no-net-6-0/
```
```bash
https://github.com/hgmauri/sample-opentelemetry
```

</details>
<br>

### *Próximas atualizações serão voltadas nas seguintes tarefas:* 

##### Branch = comentariosExplicativos

- [x] Incluir comentários explicando trechos do código. :construction:[Em construção]:construction:
- [ ] Incluir no README detalhamento de como executar a aplicação.
- [ ] Incluir no README quais ferramentas foram necessárias.

##### Branch = testesUnitarios

- [x] Incluir os Testes Unitários - XUnit.
- [x] Utilizar MOQ.
- [x] Refatorar CategoriasControllerTestesUnitarios e criar novos testes.
- [x] Utilizar MOQ - Criar Testes Unitários de ProdutosController.
- [x] Utilizar MOQ - Criar Testes Unitários de Repository.

##### Branch = testesIntegrados

- [x] Incluir os Testes Integrados - XUnit. :construction:[Em construção]:construction:
- [x] Criar a class DbMockInitializerIntegrationTest com um método Seed para popular o banco de dados em memória com dados de teste.


##### Branch = consumindoAPI

- [ ] Consumir uma API.

#
### Documentação - Version = "v1"
<br>
<details>
  <summary> <b> Imagem AutorizaController </b> <i>(clique na setinha!)</i> </summary><br>
  
![image](https://user-images.githubusercontent.com/13735095/199120458-a3f81294-0be6-4680-9c80-827e3d5a4296.png)
</details>

<br>
<details>
  <summary> <b> Imagem CategoriaController e ProdutoController </b> <i>(clique na setinha!)</i> </summary><br>

![image](https://user-images.githubusercontent.com/13735095/199120534-76a3d776-3cce-4a8e-a1e1-4e0433171572.png)
</details>

#
### Documentação - Version = "v2"
<br>
<details>
  <summary> <b> Imagem ProdutosVDoisController </b> <i>(clique na setinha!)</i> </summary><br>
  
![image](https://user-images.githubusercontent.com/13735095/199120719-b0637f04-459c-4b19-88ff-eb241e3d4353.png)
</details>

#
### Documentação - Version = "jaeger-tracing"
<br>
<details>
  <summary> <b> Imagem Jaeger </b> <i>(clique na setinha!)</i> </summary><br>
  
  ![tracing1](https://user-images.githubusercontent.com/13735095/233818051-c06edd9c-be7e-4657-8405-c96c8a462861.png) 
  
  ![tracing2](https://user-images.githubusercontent.com/13735095/233818062-bee79b21-0dff-42b9-a515-16949ef7a8ac.png)
  
  ![image](https://user-images.githubusercontent.com/13735095/233818070-a6b3f13c-a7a9-4902-b84b-b6a305242260.png)
  
  ![image](https://user-images.githubusercontent.com/13735095/233841879-2198f84e-4a65-413c-8626-4f39fdc381f2.png)
  
  ![image](https://user-images.githubusercontent.com/13735095/233841918-bd22194b-ff8a-4b3b-bd9b-2b0fc1fa9461.png)


</details>

