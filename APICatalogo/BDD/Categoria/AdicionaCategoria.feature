#language: pt-br
#encoding: utf-8

Funcionalidade: Categoria - Adicionar Categoria 
Como um usuario
Eu quero adicionar uma categoria
E que possa ter nenhum ou mais produtos cadastrados
Para que eu possa ter uma categoria cadastrada com ou sem produtos 


Cenario: Adicionar Categoria com Produtos com sucesso
Dado que estou na pagina de cadastro categorias
E quero adicionar uma nova categoria
E também quero adicionar um ou mais produtos a essa categoria
Quando eu prencher a categoria com os dados corretos e com os produtos corretos
Então a categoria de produtos deve ser cadastrada com sucesso

Cenario: Adicionar Categoria com Produtos já existentes
Dado que estou na pagina de cadastro categorias
E quero adicionar uma nova categoria com produtos já existentes
Quando eu prencher a categoria com os dados corretos e com os produtos ja existentes
Então deve ser exibida uma mensagem de erro informando que os produtos já estão cadastrados na categoria

Cenario: Adicionar Categoria sem Produtos com sucesso
Dado que estou na pagina de cadastro categorias 
E quero adicionar uma nova categoria sem produtos
Quando eu prencher a categoria com os dados corretos 
Então a categoria deve ser cadastrada com sucesso

Cenario: Adicionar Categoria já existente sem Produtos 
Dado que estou na pagina de cadastro categorias 
E quero adicionar uma nova categoria sem produtos
Quando eu prencher a categoria com os dados corretos 
Então deve ser exibida uma mensagem de erro informando que a categoria já existe