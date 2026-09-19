# Рантайм эффектов

## Модели

`EffectDefinition` — определение эффекта, принадлежащее предмету (`ItemId`). Хранится в таблице `EffectDefinitions` и загружается вместе с предметом.

`ActiveEffect` — эффект, действующий в бою. Создаётся копированием полей определения и дополняется счётчиком оставшихся раундов `RemainingRounds` (начальное значение — `DurationRounds`), числом объединённых активаций `StackCount` и изображением источника `SourceImageUrl`. Активные эффекты лежат в `MonsterFightActionState.ActiveEffects` — в состоянии боя внутри сессии активности.

`LifetimeType` задаёт истечение: `RoundBased` отсчитывает раунды, `Persistent` действует до конца боя. `SourceType` задаёт происхождение: `Potion`, `Equipment`, `Skill`.

Поля `TargetType`, `MinValue`, `MaxValue` и `NextEffectDefinitionId` хранятся и отдаются в API, но рантайм их не читает. Эффекты действуют только в пользу героя: поля монстра обработчики не меняют.

## Обработчики

Поведение эффекта реализует обработчик `IEffectHandler` — по одному на значение `EffectType`. Обработчик не хранит состояния: всё изменяемое лежит в `ActiveEffect` и в состоянии боя. Методы-хуки вызываются в фиксированных точках раунда; хук, не нужный эффекту, пуст. Метод `Stack` объединяет новую активацию с уже активным эффектом того же типа.

`EffectHandlerRegistry` сопоставляет тип с обработчиком. Для типа без обработчика он бросает `InvalidOperationException` при первом обращении в бою. Новый тип эффекта требует значения `EffectType`, класса обработчика и регистрации в `WebHost/Configs/ServiceConfig`.

| `EffectType` | Хук | Действие | `Stack` |
|---|---|---|---|
| `HEAL_HP` | `OnRoundStart` | Прибавляет `Magnitude` к HP героя, не выше максимума; пересчитывает способность | По правилу домена |
| `OVERRIDE_ABILITY_TO_MAX` | `OnStrikeClaimed`, `OnRoundEnd` | Ставит способность героя в `MaxHealth / 10` | По правилу домена |
| `STRIKE_POWER_BONUS` | `OnBeforeDamageCommit` | Прибавляет `Magnitude` к силе удара | По правилу домена |
| `UNIT_DAMAGE_DELTA` | `OnBeforeDamageCommit` | Прибавляет `Magnitude` к модификатору юнит-урона | Не меняет эффект |
| `INCOMING_STRIKE_FULL_BLOCK` | `OnIncomingStrikeConfirmed` | Если удар ещё не заблокирован и `Magnitude > 0`, с вероятностью `ChancePercent` блокирует удар и уменьшает `Magnitude` — остаток блоков | Не меняет эффект |

Правила стекования домена — [effects.md](../domain/effects.md).

## Эффекты в раунде

Раунд считает `MonsterFightService` в следующем порядке:

1. Если в состоянии боя нет эффектов с `SourceType = Equipment`, в него добавляются эффекты экипировки: определения с `SourceType = Equipment` у предметов в слотах `RIGHT_HAND` и `LEFT_HAND`.
2. Удаляются эффекты `RoundBased` с `RemainingRounds <= 0`.
3. При действии `USE_ITEM` активируются эффекты израсходованного предмета. Если активен эффект `RoundBased` того же типа, вызывается `Stack` его обработчика; иначе добавляется новый `ActiveEffect`.
4. `OnRoundStart`.
5. Броски инициативы.
6. `OnStrikeClaimed` — до расчёта преимущества по способности.
7. Расчёт стороны и силы удара.
8. `OnIncomingStrikeConfirmed` — только при ударе монстра с силой больше 0. Выставленный `StrikeBlocked` превращает раунд в ничью.
9. Определение зоны попадания.
10. `OnBeforeDamageCommit` — только при ударе героя. Сила удара меньше 1 после хука превращает раунд в ничью.
11. Урон, новое HP, пересчёт способности по HP.
12. `OnRoundEnd`.
13. `RemainingRounds` эффектов `RoundBased` уменьшается на 1.

Внутри хука эффекты обходятся по возрастанию `Priority`.

Истёкший эффект удаляется на шаге 2 следующего раунда. Поэтому состояние боя после раунда, в котором эффект истёк, содержит его с `RemainingRounds = 0`. Эффекты `Persistent` не уменьшаются и не удаляются.

## Ответ API

`MonsterFightProfile` в WebApi не включает эффекты с `SourceType = Equipment` в `ActiveEffects` состояния боя. Их действие видно только по результату раунда.
