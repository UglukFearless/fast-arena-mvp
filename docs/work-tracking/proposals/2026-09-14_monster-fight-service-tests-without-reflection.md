# Тесты MonsterFightService без рефлексии

## Изменения

## Описание

Тесты обращаются к приватным методам `MonsterFightService` через рефлексию: `MonsterFightRewardTests` — к `AggregateStackableItems`, `MonsterFightServiceLifecycleTests` — к методам жизненного цикла боя. Тесты хрупки и привязаны к деталям реализации.

Вариант: вынести агрегацию наград и жизненный цикл боя из `MonsterFightService` в самостоятельно тестируемые единицы и покрывать поведение через стабильные тестовые швы.
