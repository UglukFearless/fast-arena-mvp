# Fast Arena MVP

Небольшая браузерная игра про быстрые приключения и бои с монстрами.

Идея простая: развивать персонажа, ходить в короткие вылазки и сражаться. Проект не претендует на «серьёзный продакшен» — это исследование темы и предварительное размышление о том, с какими задачами придётся столкнуться при разработке такой игры (размышления в основном на тему доменной области - с инфраструктурной точки зрения проект примитивен).

## Технологии

- Backend: .NET 10 (ASP.NET Core, EF Core, PostgreSQL)
- Frontend: Vue 3 + TypeScript + Pinia + Vue Router
- Инфраструктура: Docker Compose

## Структура

- `backend` - серверная часть
- `frontend` - клиентская часть на Vue
- `docker-compose.yml` - запуск окружения в контейнерах

## Быстрый старт

### Вариант 1: через Docker Compose

1. Убедитесь, что установлены Docker и Docker Compose.
2. Заполните переменные в `.env` (можно взять за основу `.env.example`).
3. Запустите проект:

```bash
docker compose up --build
```

После старта приложение поднимется на портах, указанных в `.env`.

### Вариант 2: локальный запуск по частям

Backend:

```bash
cd backend/src/FastArena.WebHost
dotnet run
```

Frontend:

```bash
cd frontend
npm install
npm run serve
```

## Документация

| Файл | Содержание |
|---|---|
| [`docs/domain.md`](docs/domain.md) | Предметная область: сущности, правила, игровой цикл |
| [`docs/architecture.md`](docs/architecture.md) | Структура кода: backend, frontend, потоки данных |
| [`docs/code-style.md`](docs/code-style.md) | Проектные правила оформления кода |
| [`docs/roadmap.md`](docs/roadmap.md) | Долгосрочные направления развития |
| [`docs/todo.md`](docs/todo.md) | Текущие задачи |

**Для AI-агентов:** точка входа — [`docs/ai/index.md`](docs/ai/index.md).

## Статус

MVP/исследовательский прототип.
