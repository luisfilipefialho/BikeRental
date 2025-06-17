
# 📘 BikeRental

### 📌 Visão Geral
BikeRental é uma API REST em .NET 9, que gerencia o aluguel de motos por entregadores. Ela implementa autenticação via JWT, mensageria com RabbitMQ, persistência em PostgreSQL, upload de imagem da CNH para o MinIO, e documentação com Swagger.

---

### 🧱 Arquitetura

```
BikeRental/
│
├── src/
│   ├── BikeRental.Api/           → Camada de apresentação (Controllers, Program.cs)
│   ├── BikeRental.Application/   → Regras de negócio (Services, DTOs, Interfaces)
│   ├── BikeRental.Domain/        → Entidades e contratos de domínio
│   ├── BikeRental.Infrastructure/→ Implementações (EF Core, MinIO, Storage, DB)
│   ├── BikeRental.Messaging/     → Publicação e consumo de eventos RabbitMQ
│
├── docker-compose.yml            → Orquestração dos serviços (API, DB, RabbitMQ, MinIO)
├── README.md
└── ...
```

---

### 📦 Funcionalidades principais

- 📋 Cadastro de motos, clientes e locações
- 🧾 Upload de CNH em base64 e envio para MinIO
- 📨 Publicação de evento `BikeCreatedEvent` no RabbitMQ
- 📥 Consumer para registrar notificações de bikes do ano 2024
- 🔐 Autenticação via JWT (admin / customer)
- 🛡️ Proteção por Role com `[Authorize(Roles = "admin")]` ou `"customer"`
- 🌐 Swagger com documentação e exemplos de responses
- ✅ Tratamento global de exceções customizadas
- 🧪 Filtros por query (ex: placa da moto)

---

## 🚀 Inicialização do Projeto

### 1. Pré-requisitos

- [.NET 9.0 SDK (preview)](https://dotnet.microsoft.com)
- Docker e Docker Compose
- [Visual Studio ou VS Code]

---

### 2. Clone o Projeto

```bash
git clone https://github.com/luisfilipefialho/bikerental.git
cd bikerental
```

---

### 3. Build com Docker Compose

```bash
docker-compose up --build
```

A API estará disponível em: http://localhost:8080

Swagger: [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

### 4. Credenciais Padrão (Login)

```json
{
  "username": "admin",
  "password": "admin123"
}
```

```json
{
  "username": "customer",
  "password": "customer123"
}
```

---

### 5. Requisições protegidas com JWT

1. Acesse `/api/Auth/login`
2. Faça login e copie o token JWT retornado
3. No topo da UI do Swagger, clique em 🔒 "Authorize"
4. Insira:
   ```
   SEU_TOKEN_AQUI
   ```

---

### 6. Variáveis e configurações

```json
// appsettings.Development.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Port=5432;Database=bikerental;Username=postgres;Password=postgres"
  },
  "Minio": {
    "Endpoint": "minio:9000",
    "AccessKey": "minioadmin",
    "SecretKey": "minioadmin",
    "Bucket": "cnhs"
  },
  "Jwt": {
    "SecretKey": "SUA_CHAVE_SECRETA_COM_32_BYTES_AQUI",
    "Issuer": "BikeRentalAPI",
    "Audience": "BikeRentalUsers"
  },
  "RabbitMQ": {
    "Host": "rabbitmq",
    "User": "guest",
    "Pass": "guest"
  }
}
```

> Use um segredo com no mínimo 256 bits (32 caracteres) para `Jwt:SecretKey`.

---

## ✅ Observações finais

- O upload de imagens da CNH aceita base64 PNG ou BMP
- Todas as entidades utilizam identificadores como string (não GUID)
- O sistema de aluguel aplica regras de plano, multa, e diárias extras conforme regras do desafio
- Há tratamento para exceções como:
  - `EntityNotFoundException`
  - `DomainConflictException`
  - `HasRentalException`
  - `InvalidInputException`
