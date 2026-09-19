# Frontend

## Сборка

Vue 3 и TypeScript, сборка Vue CLI (`vue-cli-service`), стили на SCSS. Псевдоним `@` указывает на `src/`. Компоненты пишутся в `<script setup lang="ts">`.

## Структура `src/`

- `pages/` — экраны маршрутов. Страница выбирает layout и собирает экран из компонентов; содержимое экрана может целиком лежать в компоненте (`Shop.vue` → `components/Shop/ShopPage.vue`).
- `layouts/` — каркасы страниц: `BaseLayout` с шапкой, `BlankLayout` для входа и регистрации, `FightLayout` для боя.
- `components/` — компоненты, сгруппированные по экранам; общие лежат в `shared/`.
- `stores/` — хранилища Pinia в options-стиле.
- `services/` — `auth-service` (токен в `localStorage`), `action-route-service` (маршрут по типу действия).
- `api/clients.ts` — сгенерированный клиент API; вручную не правится.
- `model/` — клиентские типы, отсутствующие в API.
- `utils/` — `ApiSettings` с базовым адресом API и обёртка `authFetch`.

## Маршруты

Маршруты заданы в `router/index.ts` в режиме history, страницы загружаются лениво.

| Путь | Страница |
|---|---|
| `/` | `Barracks` |
| `/create-hero` | `CreateHero` |
| `/login` | `Login` |
| `/registration` | `Registration` |
| `/arena` | `Arena` |
| `/fight` | `Fight` |
| `/statistic` | `Statistic` |
| `/hero-info/:id` | `HeroInfo` |
| `/shop` | `Shop` |

Глобальный guard перед каждым переходом проверяет токен запросом `GET /api/health/secret-ping` и удаляет токен при ответе с кодом, отличным от 200. Затем он проверяет флаги маршрута: `meta.requiresAuth` без токена ведёт на `/login`, `meta.requiresAnonimous` с токеном — на `/`. Для авторизованного пользователя guard запрашивает текущую сессию активности: при наличии сессии ведёт на маршрут её текущего действия, без сессии маршруты действий ведут на `/`.

Соответствие типа действия сессии и маршрута задано дважды: функцией `getRoutePathByActionType` в `router/index.ts` и `ActionRouteService`. Список маршрутов действий — `getAllActivitiesRoutes` там же. Новый тип действия требует правки всех трёх мест.

## Обращение к API

Базовый адрес `ApiSettings.BaseUrl` складывается из `VUE_APP_API_BASE_HOST` и `VUE_APP_API_BASE_PORT`, заданных при сборке; пустой адрес означает запросы к своему origin.

Клиенты API создаются с адресом `ApiSettings.BaseUrl` и `authFetch`; без `authFetch` создаётся только клиент регистрации и входа. Хранилища создают клиент на каждый вызов, компоненты — один раз при создании. Обращения к API выполняют действия хранилищ Pinia; исключения — компоненты `HeroInfo`, `HeroInventory` и `ShopPage`, которые вызывают клиентов напрямую.

Два места формируют запросы вручную, минуя классы клиентов: хранилище `shop` — через `authFetch.fetch`, проверка токена в `auth-service` — через `fetch`. Изменение этих эндпоинтов перегенерация клиента не покрывает.
