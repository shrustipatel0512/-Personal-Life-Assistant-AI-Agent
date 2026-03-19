# Architecture Blueprint

## High-Level Design

The system follows Clean Architecture with CQRS:

- `Domain`: core business entities and enums
- `Application`: use cases, MediatR requests/handlers, interfaces, DTOs
- `Infrastructure`: persistence, authentication, AI provider integration, schedulers
- `Api`: HTTP endpoints, auth, rate limiting, Swagger
- `Frontend`: Angular dashboard, planner, tasks, chat

## Key Flows

### Task Creation

1. Angular posts task payload to `POST /api/tasks`
2. API sends `CreateTaskCommand` through MediatR
3. Application calls `IAiService.AnalyzeTaskAsync`
4. Gemini returns category, priority, and reasoning
5. Task is stored in PostgreSQL
6. Response returns enriched task data to UI

### Daily Planning

1. Frontend calls `POST /api/planner/generate`
2. Planner query loads pending tasks and user context
3. Gemini produces schedule blocks
4. Planner result is returned for calendar rendering

### Chat Assistant

1. User sends message from Angular chat
2. API loads recent chat history and passes context to Gemini
3. Assistant reply is stored in `chat_history`
4. UI renders response and optional suggested actions

## Recommended Deployment Topology

- Angular app: static hosting on Vercel, Netlify, Azure Static Web Apps, or Nginx
- API: Azure App Service, Azure Container Apps, AWS ECS, or Kubernetes
- PostgreSQL: Azure Database for PostgreSQL or managed cloud equivalent
- Queue/background jobs: Quartz.NET or Hangfire
- Notifications:
  - Email: SendGrid / Mailgun
  - SMS: Twilio
  - Push: Firebase Cloud Messaging

## Cross-Cutting Concerns

- JWT authentication with role claims
- Rate limiting per user/IP
- Centralized exception handling with Problem Details
- Structured logging with Serilog
- OpenTelemetry tracing for AI and database spans
- Caching for dashboard summaries and planner reads
