# Scalability and Evolution Plan

## Immediate Improvements

- Add Redis caching for dashboard summaries and active chat context
- Use outbox pattern for notifications and AI-event publishing
- Add background summarization for long chat histories
- Store embeddings for semantic memory and retrieval

## Suggested Bounded Contexts

- Identity
- Productivity
- Finance
- Wellness
- Notifications
- AI Orchestration

## Microservices Migration Path

### Phase 1

- Keep modular monolith
- Split by folders, contracts, and database schemas
- Publish internal domain events

### Phase 2

- Extract Notifications service first
- Extract AI Orchestration service second
- Introduce message broker such as RabbitMQ or Azure Service Bus

### Phase 3

- Move chat memory and recommendation pipelines into dedicated AI service
- Introduce API gateway and BFF for Angular app
- Add read models optimized for dashboard and planner

## Real-World Enhancements

- Calendar sync with Google Calendar and Outlook
- Voice assistant mode with speech-to-text and text-to-speech
- WhatsApp / Telegram reminder integration
- OCR for receipt expense capture
- Wearable integrations for health signals
- RAG over personal documents, notes, and emails with consent
