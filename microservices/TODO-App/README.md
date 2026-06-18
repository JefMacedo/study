# TODO App API

API de gerenciamento de tarefas com suporte a subtarefas e auto-conclusão.

## Arquitetura

Projeto estruturado em camadas seguindo **Clean Architecture** e princípios **SOLID**:

- **Domain** - Entidades principais (TaskItem, SubTask)
- **Application** - Commands, Queries, Validators e Handlers (MediatR)
- **Infrastructure** - Persistência com EF Core + PostgreSQL
- **API** - Controllers, Middleware, Configuração de DI

## Tecnologias

- .NET 10
- C# 14
- Entity Framework Core 10
- PostgreSQL 17
- MediatR (Command/Query Pattern)
- FluentValidation
- Swagger/OpenAPI

## Como Rodar

### 1. Iniciar PostgreSQL com Docker Compose

```bash
cd d:\dev\study\microservices\TODO-App
docker-compose up -d
```

Verifique se o PostgreSQL está rodando:
- Conexão: `localhost:5432`
- Database: `todo_app`
- User: `postgres`
- Password: `postgres`

### 2. Restaurar Dependencies e Rodar

```bash
cd src\TodoApp.Api
dotnet restore
dotnet build
dotnet run
```

A API iniciará em:
- HTTP: `http://localhost:5005`
- HTTPS: `https://localhost:5006`

### 3. Acessar Swagger

Abra no navegador:
```
https://localhost:5006/swagger
```

## Funcionalidades

### Criar Tarefa com Subtarefas

```bash
curl -X POST https://localhost:5006/api/tasks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Ir ao mercado",
    "description": "Compra para o café da manhã",
    "dueDate": "2026-06-20T00:00:00Z",
    "subTasks": [
      {"title": "Leite", "description": "Ninho"},
      {"title": "Pão frances", "description": null},
      {"title": "Café", "description": "Pacote 250g Pilão"}
    ]
  }'
```

### Buscar Tarefa com Subtarefas

```bash
curl https://localhost:5006/api/tasks/{id}
```

### Completar Subtarefa

```bash
curl -X PUT https://localhost:5006/api/tasks/{taskId}/subtasks/{subTaskId}/complete
```

**Lógica**: Quando **todas** as subtarefas forem concluídas, a tarefa é automaticamente marcada como concluída.

## Migração do Banco

As migrações são aplicadas automaticamente no startup via `Database.Migrate()`.

Para gerar uma nova migração (se necessário):

```bash
cd microservices/TODO-App
dotnet ef migrations add add-subtask-migration -s src/TodoApp.Api -p src/TodoApp.Infrastructure
```

Para atualizar a base de dados manualmente (se necessário):

```bash
cd microservices/TODO-App
dotnet ef database update -s src/TodoApp.Api -p src/TodoApp.Infrastructure
```

## Estrutura de Pastas

```
TODO-App/
├── src/
│   ├── TodoApp.Domain/
│   │   └── Entities/
│   │       ├── TaskItem.cs
│   │       └── SubTask.cs
│   ├── TodoApp.Application/
│   │   ├── Commands/
│   │   │   ├── AddSubTask/
│   │   │   ├── CreateTask/
│   │   │   └── CompleteSubTask/
│   │   ├── Queries/
│   │   │   └── GetTask/
│   │   ├── Interfaces/
│   │   │   └── IAppDbContext.cs
│   │   └── Validators/
│   ├── TodoApp.Infrastructure/
│   │   └── Persistence/
│   │       └── AppDbContext.cs
│   └── TodoApp.Api/
│       ├── Controllers/
│       │   └── TasksController.cs
│       ├── ExceptionHandling/
│       │   └── ExceptionMiddleware.cs
│       ├── Program.cs
│       └── appsettings.json
└── docker-compose.yml
```

## Próximos Passos

### API

- [ ] Listar todas as tarefas
- [ ] Listar tarefa pelo titulo ou por Id
- [ ] Marcar uma tarefa como concluída
- [ ] Editar uma tarefa
- [ ] Excluir uma tarefa
- [ ] Listar todas as subtasks
- [ ] Listar uma subtask pelo titulo ou por Id
- [ ] Editar uma subtask
- [ ] Excluir uma subtask

### Melhoria do projeto

- [ ] Adicionar testes unitários (xUnit, Moq, FluentAssertions)
- [ ] Implementar paginação
- [ ] Adicionar autenticação JWT
- [ ] Implementar logging estruturado
- [ ] Adicionar rate limiting
- [ ] Containerizar a API com Docker
