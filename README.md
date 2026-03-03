# My Story Team API

A .NET Web API backend for **My Story Team** — a collaborative storytelling and canvas application with AI-powered conversations.

## Website

There is also a website for this project at: **[https://fran-klasic.github.io/MyStoryTeamV3/](https://fran-klasic.github.io/MyStoryTeamV3/)**

---

## Overview

My Story Team API provides authentication, canvas management, team conversations, and AI-powered chat functionality. It is designed to work with the My Story Team frontend application.

## Tech Stack

- **.NET 10.0** (ASP.NET Core Web API)
- **Entity Framework Core 10** with SQL Server
- **JWT Bearer Authentication**
- **OpenAI API** (for AI conversations)
- **BCrypt.Net** (password hashing)
- **Newtonsoft.Json** (JSON serialization with custom converters)

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or Azure)
- OpenAI API key

## Configuration

### Environment Variables

| Variable              | Description                                                 |
| --------------------- | ----------------------------------------------------------- |
| `DATABASE_CONNECTION` | SQL Server connection string (overrides `appsettings.json`) |
| `OPENAI_KEY`          | OpenAI API key (overrides `appsettings.json`)               |

### User Secrets (Development)

The project uses .NET User Secrets for local development. Configure:

- **JwtConfig** (Issuer, Audience, Key) — in `appsettings.Development.json` or User Secrets
- **ConnectionStrings:Database** — SQL Server connection string
- **OpenAI:ApiKey** — OpenAI API key

### Production

- JWT and database settings are configured in `appsettings.Production.json` and environment variables.
- CORS is restricted to `https://fran-klasic.github.io` in production.

## Getting Started

### 1. Clone and Restore

```bash
git clone <repository-url>
cd MyStoryTeamAPI
dotnet restore
```

### 2. Configure Database

Ensure your SQL Server connection string is set (via `appsettings.json`, User Secrets, or `DATABASE_CONNECTION` environment variable). Apply migrations if using EF Core migrations:

```bash
dotnet ef database update
```

### 3. Run the API

```bash
dotnet run
```

- **HTTP:** http://localhost:5116
- **HTTPS:** https://localhost:7109

---

## API Endpoints

### Authentication (`/api/auth`)

| Method | Endpoint                  | Auth | Description                             |
| ------ | ------------------------- | ---- | --------------------------------------- |
| GET    | `/api/auth/username/{id}` | No   | Get username by user ID                 |
| POST   | `/api/auth/login`         | No   | Login and receive JWT token             |
| POST   | `/api/auth/register`      | No   | Register new user and receive JWT token |
| GET    | `/api/auth/test`          | Yes  | Test authenticated access               |
| GET    | `/api/auth/user`          | Yes  | Get current user info                   |

### Canvas (`/api/auth/canvas`)

| Method | Endpoint                  | Description              |
| ------ | ------------------------- | ------------------------ |
| GET    | `/api/auth/canvas`        | Get all user canvases    |
| GET    | `/api/auth/canvas/{id}`   | Get canvas details by ID |
| POST   | `/api/auth/canvas`        | Create new canvas        |
| PUT    | `/api/auth/canvas`        | Update canvas            |
| DELETE | `/api/auth/canvas`        | Delete canvas            |
| GET    | `/api/auth/canvas/public` | Get all public canvases  |

### Conversations (`/api/auth/conversations`)

| Method | Endpoint                            | Description                    |
| ------ | ----------------------------------- | ------------------------------ |
| GET    | `/api/auth/conversations`           | Get all conversations          |
| GET    | `/api/auth/conversations/{id}`      | Get messages in a conversation |
| GET    | `/api/auth/conversations/{id}/name` | Get conversation name          |
| POST   | `/api/auth/conversations`           | Create new conversation        |
| POST   | `/api/auth/conversations/{id}`      | Create message in conversation |
| PUT    | `/api/auth/conversations/{id}`      | Add user to conversation       |
| PUT    | `/api/auth/conversations/{id}/name` | Update conversation name       |

### AI Conversations (`/api/auth/ai`)

| Method | Endpoint               | Description                         |
| ------ | ---------------------- | ----------------------------------- |
| GET    | `/api/auth/ai`         | Get all AI conversations            |
| GET    | `/api/auth/ai/{id}`    | Get messages in AI conversation     |
| POST   | `/api/auth/ai`         | Create new AI conversation          |
| POST   | `/api/auth/ai/message` | Send message to AI and get response |
| PUT    | `/api/auth/ai`         | Update AI conversation title        |

### Test (`/api/test`)

| Method | Endpoint    | Description                           |
| ------ | ----------- | ------------------------------------- |
| GET    | `/api/test` | Health check (returns "Hello World!") |

---

## Project Structure

```
MyStoryTeamAPI/
├── Controllers/          # API controllers
├── Db/                   # EF Core DbContext
├── Models/               # Request/response and DB models
│   ├── App/              # App configuration (e.g. JwtConfig)
│   ├── Canvas/           # Canvas element types
│   ├── Db/               # Database entities
│   ├── Requests/         # API request DTOs
│   └── Responses/        # API response DTOs
├── Repository/           # Data access layer
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
└── appsettings.Production.json
```
