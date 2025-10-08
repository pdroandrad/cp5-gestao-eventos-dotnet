# 🎭 API de Eventos Culturais

## 📌 Descrição do Projeto

A **API de Eventos Culturais** é uma aplicação desenvolvida em **ASP.NET Core Web API** com **MongoDB** como banco de dados, projetada para gerenciar informações sobre eventos culturais.  

O sistema permite **autenticação via JWT** e operações CRUD completas sobre os eventos, garantindo segurança, escalabilidade e fácil integração com outras aplicações.

A API foi construída com foco em **boas práticas REST**, incluindo:
- Organização clara dos endpoints.  
- Uso adequado dos status HTTP.  
- Conexão com banco de dados **MongoDB Atlas**.  
- Implementação de autenticação com **JWT (JSON Web Token)**.  

---

## 👨‍💻 Integrantes

- Pedro Abrantes Andrade | RM558186  
- Ricardo Tavares de Oliveira Filho | RM556092  
- Victor Alves Carmona | RM555726  

---

## 🚀 Tecnologias Utilizadas

- ASP.NET Core 8.0 Web API  
- C#  
- MongoDB (via MongoDB.Driver)  
- JWT Authentication  
- Swagger / OpenAPI  
- Visual Studio 2022 ou superior  
- .NET CLI  

---

## 📂 Instalação e Execução

### ✅ Pré-requisitos

- .NET 8.0 ou superior  
- Visual Studio 2022 ou Visual Studio Code  
- Conta no [MongoDB Atlas](https://www.mongodb.com/cloud/atlas) ou instância local do MongoDB  

---

### ▶️ Passos para execução

1. Clone o repositório:
   ```bash
   git clone https://github.com/pdroandrad/cp5-gestao-eventos-dotnet.git
   ```

2. Acesse a pasta do projeto:
   ```bash
   cd gestaoEventos
   ```

3. Configure a string de conexão no arquivo appsettings.json adicionando usuario, senha, cluster e key:
   ```bash
   {
     "ConnectionStrings": {
       "MongoDB": "mongodb+srv://<usuario>:<senha>@<cluster>.mongodb.net/EventosDB"
     },
     "Jwt": {
       "Key": "chave-secreta-super-segura",
       "Issuer": "GestaoEventosDB",
       "Audience": "Eventos"
     }
   }
   ```

4. Execute a aplicação:
   ```bash
   dotnet run
   ```

5. Acesse o Swagger para testar os endpoints (deve abrir automaticamente):
   ```bash
   https://localhost:7090/swagger
   ```

---

## 🔐 Autenticação JWT

A API utiliza **autenticação baseada em token JWT** para proteger os endpoints.  

### Endpoint de Login

**POST /api/auth/login**

**Body:**
```json
{
  "username": "admin",
  "password": "12345"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI..."
}
```

Utilize o token retornado no cabeçalho **Authorization** das requisições:
Authorization: Bearer + <espaço> + token gerado

---

## 🎫 Endpoints da API

### 🔑 Autenticação

| Método | Endpoint           | Descrição                              |
|:------:|:------------------:|:--------------------------------------|
| POST   | `/api/auth/login`  | Realiza o login e retorna o token JWT. |

---

### 🎭 Eventos

| Método | Endpoint                | Descrição                                      |
|:------:|:-----------------------:|:----------------------------------------------|
| GET    | `/api/eventos`          | Lista todos os eventos culturais cadastrados.  |
| GET    | `/api/eventos/{id}`     | Retorna os detalhes de um evento específico.   |
| POST   | `/api/eventos`          | Cria um novo evento cultural.                  |
| PUT    | `/api/eventos/{id}`     | Atualiza os dados de um evento existente.      |
| DELETE | `/api/eventos/{id}`     | Exclui um evento cultural pelo ID.             |

---

## 📦 Exemplos de Requisição

### 🔹 **POST /api/eventos**

```json
{
  "titulo": "Festival de Música Brasileira",
  "descricao": "Um evento cultural que celebra a diversidade e riqueza da música brasileira, com apresentações ao vivo de artistas independentes e renomados.",
  "data": "2025-11-15T18:00:00Z",
  "local": "Parque Ibirapuera, São Paulo - SP",
  "categoria": "Cultural",
  "capacidadeMaxima": 5000
}
```

### 🔹 **PUT /api/eventos/{id}**

```json
{
  "titulo": "Festival de Música Brasileira",
  "descricao": "Um evento cultural que celebra a diversidade e riqueza da música brasileira, com apresentações ao vivo de artistas independentes e renomados.",
  "data": "2025-11-15T18:00:00Z",
  "local": "Estádio Morumbi, São Paulo - SP",
  "categoria": "Cultural",
  "capacidadeMaxima": 30000
}
```

---

## 🗄️ Modelo de Dados (MongoDB)

Cada evento é armazenado como um documento na coleção **`eventos`** com o seguinte formato:

```json
{
  "_id": "ObjectId",
  "titulo": "string",
  "descricao": "string",
  "data": "DateTime",
  "local": "string",
  "categoria": "string",
  "capacidadeMaxima": "int",
  "dataCriacao": "DateTime"
}


