# Инфраструктура

## Docker Compose

`docker-compose.yml` поднимает четыре сервиса:

- `fa_postgres` — PostgreSQL; наружу порт не публикуется;
- `pgadmin` — pgAdmin на порту `PGADMIN_PORT`;
- `app` — backend на порту `APP_HTTP_PORT`;
- `fastarena-front` — frontend; порт открыт только внутри сети.

Сеть `postgres` связывает все сервисы, сеть `internet` — `app` и `pgadmin`. `app` стартует после того, как healthcheck `fa_postgres` (`pg_isready`) проходит, и после запуска `fastarena-front`.

Переменные `.env` (образец — `.env.example`) попадают в конфигурацию backend через переменные окружения контейнера `app`:

| Конфигурация backend | Источник |
|---|---|
| `ConnectionStrings:Default` | `DB_HOST`, `POSTGRES_PORT`, `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB` |
| `AuthOptions:SecretKey` | `AUTH_SECRET_KEY` |
| `AuthOptions:HashSecret` | `HASH_SECRET` |
| `ReverseProxy:Clusters:frontend_cluster:Destinations:main:Address` | `http://fastarena-front:` и `FRONTEND_PORT` |

`APP_HTTPS_PORT` из `.env.example` compose не использует.

## Образы

Образ backend (`backend/Dockerfile`) собирается SDK .NET 10 командой `dotnet publish` проекта `FastArena.WebHost` и запускается на runtime-образе ASP.NET на порту 8080.

Образ frontend (`frontend/Dockerfile`) собирает приложение `npm run build` на Node и раздаёт `dist` через `http-server` на порту `FRONTEND_PORT`; неизвестные пути `http-server` возвращает к корню, что обслуживает маршруты SPA. Аргументы сборки `API_BASE_HOST` и `API_BASE_PORT` становятся переменными `VUE_APP_API_BASE_HOST` и `VUE_APP_API_BASE_PORT`. Compose передаёт в них `API_BASE_HOST` и `APP_HTTP_PORT` из `.env`, поэтому в образе compose адрес API абсолютный.

## Локальный запуск

`appsettings.json` содержит заглушки строки подключения и секретов; при запуске через `dotnet run` их задают переменными окружения или локальной конфигурацией. Профиль `http` в `launchSettings.json` поднимает backend на `http://localhost:5204`. Адрес назначения прокси по умолчанию — `http://localhost:8080/`, порт dev-сервера Vue CLI (`npm run serve`). При запуске dev-сервера без `VUE_APP_API_BASE_HOST` приложение открывается через адрес backend.

Команды запуска приведены в [README](../../README.md).

## Генерация API-клиента

`backend/src/FastArena.ApiClientGenerator/gen-ts.bat` запускает генератор против OpenAPI-документа `http://localhost:8100/swagger/FastArenaAPI/swagger.json` — backend должен быть доступен на порту 8100, например запущен командой `dotnet run --urls http://localhost:8100`. Результат записывается в `client-build/api-clients.ts` рядом с генератором; в `frontend/src/api/clients.ts` его копируют вручную. После генерации программа ждёт нажатия клавиши.

## Жизненный цикл базы данных

Схему и начальные данные создаёт backend при старте — [backend.md](backend.md). В compose база живёт в томе `local_postgres_data` и переживает пересоздание контейнеров. Пересоздание базы — удаление базы или тома и повторный запуск backend.
