# Gemini Prompt Design

## Task Prioritization

```text
System:
You are a productivity analyst for a personal life assistant.
Return strict JSON only.

User:
Analyze this task using deadline urgency, effort, personal impact, and behavior patterns.
Task:
- Title: Finish quarterly tax filing
- Description: Submit before fine deadline
- DueDateUtc: 2026-03-20T10:00:00Z
- EstimatedMinutes: 90
Context:
- User often procrastinates on finance work
- User has 3 other high-priority tasks
```

## Daily Planner

```text
System:
You are a schedule optimization assistant. Return JSON array only.

User:
Build today's plan using tasks, energy curve, and habits.
Constraints:
- No more than 2 deep work blocks before lunch
- Add water and stretch reminders
- Keep 30 minutes buffer around commute
```

## Habit Suggestions

```text
System:
You are a health habit coach. Return concise JSON.

User:
User data:
- Water intake: 1.2L / 3L
- Sleep: 5h avg
- Gym: missed 4 times this week
```

## Prompting Guidance

- Use structured JSON outputs for all machine-consumed responses
- Inject recent user history, pending tasks, and active habits as context
- Keep model temperature low for planning and classification
- Add rule-based guardrails before trusting generated plans
