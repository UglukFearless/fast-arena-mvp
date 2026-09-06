# Fast Arena — Инструкции для Copilot

## С чего начать

Перед любой задачей прочитать [`docs/ai/index.md`](../docs/ai/index.md).
Он направит к нужной документации в зависимости от типа задачи.

## Канонические документы

| Что нужно | Файл |
|---|---|
| Предметная область продукта, сущности, правила | [`docs/domain.md`](../docs/domain.md) |
| Структура кода, модули, поток данных | [`docs/architecture.md`](../docs/architecture.md) |
| Стиль кода, специфичный для проекта | [`docs/code-style.md`](../docs/code-style.md) |
| Долгосрочное направление | [`docs/roadmap.md`](../docs/roadmap.md) |
| Текущие задачи | [`docs/todo.md`](../docs/todo.md) |
| Границы и гранулярность документации | [`docs/ai/doc-writing-boundaries.md`](../docs/ai/doc-writing-boundaries.md) |
| Команды и типичные ошибки | [`docs/ai/commands.md`](../docs/ai/commands.md), [`docs/ai/pitfalls.md`](../docs/ai/pitfalls.md) |

## Ограничения

- Не придумывать правила, отсутствующие в `docs/domain.md`.
- Следовать `docs/code-style.md` для решений по стилю, специфичных для репозитория.
- Держать изменения в рамках текущей задачи.
- Допущения MVP — это текущее поведение; пункты roadmap — нет.
- При сомнении в бизнес-смысле — спрашивать до изменения поведения.
