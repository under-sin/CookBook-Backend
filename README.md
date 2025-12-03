# 🍳 MyRecipeBook

API RESTful desenvolvida em .NET 8 para gerenciamento de receitas culinárias, implementando Clean Architecture e Domain-Driven Design (DDD) com foco em boas práticas, testabilidade e escalabilidade.

## 🏗️ Arquitetura

O projeto segue **Clean Architecture** dividido em 4 camadas bem definidas:

- **Domain**: Entidades, interfaces de repositórios, value objects e regras de negócio core
- **Application**: Casos de uso, validações com FluentValidation, AutoMapper e lógica de aplicação
- **Infrastructure**: Implementação de repositórios (EF Core), segurança (JWT, BCrypt), Azure Storage, Service Bus e migrations (FluentMigrator)
- **API**: Controllers, filters, middlewares, background services e configuração de DI

### Padrões Aplicados
- **Repository Pattern** com Unit of Work
- **Use Cases** (Command/Query)
- **Dependency Injection** nativa do .NET
- **Background Services** para processamento assíncrono

## 🛠️ Stack Tecnológica

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM para MySQL
- **Dapper** - Queries otimizadas
- **FluentMigrator** - Controle de versões do banco de dados
- **AutoMapper** - Mapeamento objeto-objeto
- **FluentValidation** - Validação de entradas

### Segurança & Autenticação
- **JWT (JSON Web Tokens)** - Autenticação stateless
- **BCrypt.Net** - Hash de senhas

### Infraestrutura Cloud (Azure)
- **Azure Blob Storage** - Armazenamento de imagens de receitas
- **Azure Service Bus** - Mensageria para exclusão assíncrona de usuários

### Banco de Dados
- **MySQL 8.0** - Banco de dados principal
- **In-Memory Database** - Testes de integração

### Testes
- **xUnit** - Framework de testes
- **WebApplicationFactory** - Testes de integração
- **Cobertura**: 108 testes (Unit + Integration)

### Documentação
- **Swagger/OpenAPI** - Documentação interativa da API

## 📋 Funcionalidades Principais

### Usuários
- Registro e autenticação (JWT)
- Perfil (consulta/atualização)
- Alteração de senha
- Exclusão de conta (processamento assíncrono via Service Bus)

### Receitas
- CRUD completo de receitas
- Upload de imagem de capa (Azure Blob Storage)
- Filtros avançados (título, ingredientes, tempo de preparo, dificuldade)
- Dashboard com estatísticas
- Suporte a múltiplos idiomas (pt-BR, en, fr, pt-PT)

### Características Técnicas
- **IDs encriptados** (Sqids) expostos na API
- **Validação de tipos de arquivo** (File.TypeChecker)
- **Middleware de cultura** para internacionalização
- **Exception handling** centralizado
- **Background worker** para tarefas assíncronas

## 🚀 Como Executar

### Pré-requisitos
- .NET 8 SDK
- Docker (para MySQL)
- Conta Azure (Storage e Service Bus) ou configurar mocks

### Configuração

1. **Subir o banco de dados**:
```bash
docker-compose up -d
```

2. **Configurar variáveis de ambiente** (appsettings.Development.json):
```json
{
  "ConnectionStrings": {
    "Connection": "Server=localhost;Database=MyRecipeBookDB;Uid=root;Pwd=root;"
  },
  "Settings": {
    "Jwt": {
      "SigningKey": "sua-chave-secreta",
      "ExpirationTimeMinutes": 60
    },
    "BlobStorage": {
      "Azure": "sua-connection-string"
    },
    "ServiceBus": {
      "DeleteUserAccount": "sua-connection-string"
    }
  }
}
```

3. **Executar a aplicação**:
```bash
dotnet run --project src/Backend/MyRecipeBook.API
```

4. **Acessar o Swagger**: `https://localhost:5001/swagger`

### Testes

```bash
# Todos os testes
dotnet test

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 📊 Diferenciais Técnicos

- **Processamento assíncrono** com Azure Service Bus para operações custosas
- **Armazenamento em nuvem** com Azure Blob Storage
- **Migrations versionadas** para controle de esquema do banco
- **Testes de integração** usando WebApplicationFactory (in-memory database)
- **Clean Architecture** garantindo baixo acoplamento e alta coesão
- **Segurança** com tokens JWT e hashing BCrypt
- **Internacionalização** com suporte a múltiplos idiomas
- **API documentada** com Swagger/OpenAPI