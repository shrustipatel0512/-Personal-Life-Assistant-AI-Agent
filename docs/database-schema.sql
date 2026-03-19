create table users (
    id uuid primary key,
    full_name varchar(200) not null,
    email varchar(320) not null unique,
    password_hash varchar(512) not null,
    time_zone varchar(100) not null default 'UTC',
    role varchar(50) not null default 'User',
    preferences_json jsonb not null default '{}'::jsonb,
    created_at_utc timestamptz not null,
    updated_at_utc timestamptz null
);

create table tasks (
    id uuid primary key,
    user_id uuid not null references users(id) on delete cascade,
    title varchar(200) not null,
    description varchar(2000) null,
    category int not null,
    priority int not null,
    is_completed boolean not null default false,
    due_date_utc timestamptz null,
    estimated_minutes int not null default 0,
    ai_reasoning varchar(4000) not null default '',
    created_at_utc timestamptz not null,
    updated_at_utc timestamptz null
);

create index ix_tasks_user_due on tasks(user_id, due_date_utc);

create table habits (
    id uuid primary key,
    user_id uuid not null references users(id) on delete cascade,
    name varchar(120) not null,
    frequency varchar(50) not null,
    target_value int not null,
    current_value int not null,
    unit varchar(30) not null,
    ai_suggestion varchar(2000) not null default '',
    created_at_utc timestamptz not null,
    updated_at_utc timestamptz null
);

create table expenses (
    id uuid primary key,
    user_id uuid not null references users(id) on delete cascade,
    amount numeric(12,2) not null,
    currency varchar(10) not null,
    category varchar(120) not null,
    occurred_on_utc timestamptz not null,
    notes varchar(1000) null,
    created_at_utc timestamptz not null,
    updated_at_utc timestamptz null
);

create table notifications (
    id uuid primary key,
    user_id uuid not null references users(id) on delete cascade,
    channel varchar(30) not null,
    message varchar(1000) not null,
    scheduled_for_utc timestamptz not null,
    is_sent boolean not null default false,
    metadata_json jsonb not null default '{}'::jsonb,
    created_at_utc timestamptz not null,
    updated_at_utc timestamptz null
);

create table chat_history (
    id uuid primary key,
    user_id uuid not null references users(id) on delete cascade,
    role varchar(30) not null,
    message varchar(8000) not null,
    intent varchar(100) not null,
    context_snapshot_json jsonb not null default '{}'::jsonb,
    created_at_utc timestamptz not null,
    updated_at_utc timestamptz null
);

create index ix_chat_history_user_created on chat_history(user_id, created_at_utc desc);
