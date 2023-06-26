# Aplicação de Conta Corrente

Esta é uma aplicação de conta corrente que permite realizar operações como criar uma conta, realizar depósitos, saques e transferências, consultar saldo e extrato da conta. A aplicação utiliza o padrão de arquitetura DDD (Domain-Driven Design) e armazenamento em cache com o uso do MemoryCache.

## Funcionalidades

A aplicação oferece as seguintes funcionalidades:

- Criar uma conta corrente: Permite criar uma nova conta corrente informando o nome do titular e o limite da conta.
- Realizar um depósito: Permite realizar um depósito em uma conta corrente existente informando o ID da conta e o valor a ser depositado.
- Realizar um saque: Permite realizar um saque em uma conta corrente existente informando o ID da conta e o valor a ser sacado.
- Realizar uma transferência entre contas: Permite transferir um valor de uma conta corrente para outra informando os IDs das contas de origem e destino, e o valor a ser transferido.
- Consultar o saldo da conta: Permite consultar o saldo atual de uma conta corrente informando o ID da conta.
- Consultar o extrato da conta por período: Permite consultar o extrato de movimentações de uma conta corrente em um determinado período informando o ID da conta, a data de início e a data de fim.
- Consultar o extrato da conta por tipo de operação: Permite consultar o extrato de movimentações de uma conta corrente filtrando por tipo de operação (crédito ou débito) informando o ID da conta e o tipo desejado.

## Arquitetura

A aplicação segue o padrão de arquitetura DDD (Domain-Driven Design), que organiza o código em diferentes camadas com responsabilidades específicas. As principais camadas são:

- **Domain**: Contém as entidades de domínio, como a entidade `Account` (conta corrente) e `Movement` (movimentação), que representam os conceitos centrais da aplicação.
- **Domain.Service**: Contém os serviços de aplicação que implementam as regras de negócio e orquestram as operações da aplicação. Aqui encontramos a interface `IAccountService`, que define os métodos disponíveis para manipular as contas correntes.
- **Domain.Repository**: Contém as implementações concretas das interfaces definidas nas camadas superiores. Aqui encontramos os repositórios, como `AccountRepository` e `MovementRepository`, que lidam com o armazenamento de dados em cache usando o MemoryCache.

## Configuração

Para executar a aplicação, é necessário ter o ambiente .NET configurado. Certifique-se de ter o SDK do .NET instalado na sua máquina.

1. Clone o repositório do projeto para o seu ambiente local.
2. Abra o terminal na pasta raiz do projeto.
3. Execute o comando `dotnet run` para iniciar a aplicação.
4. A aplicação estará rodando em `http://localhost:5000` ou `https://localhost:5001`, dependendo das configurações.

## Documentação da API

A API é documentada utilizando o Swagger, que fornece uma interface interativa para testar e explorar os endpoints disponíveis. Para acessar a documentação da API, abra o navegador e vá para `http://localhost:5000/swagger` ou `https://localhost:5001/swagger`.

## Dependências

A aplicação utiliza as seguintes dependências:

- ASP.NET Core
- FluentValidation: Biblioteca para validação de dados com suporte a notificações de validação.
- Swashbuckle (Swagger): Biblioteca para geração de documentação da API.

Certifique-se de restaurar as dependências antes de executar a aplicação.

## Considerações Finais

Esta aplicação de conta corrente oferece uma solução para realizar operações bancárias básicas usando o padrão de arquitetura DDD. Ela é flexível e pode ser estendida para incluir novas funcionalidades de acordo com as necessidades do projeto.

Aproveite a aplicação como ponto de partida e personalize-a conforme suas necessidades específicas.
