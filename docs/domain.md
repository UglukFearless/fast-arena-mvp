# Domain

## Purpose

Fast Arena MVP is a browser game prototype about short combat runs, hero growth, and lightweight progression. This document describes the project domain in product terms rather than code terms.

## Product Idea

- The player manages a hero.
- The hero enters short adventures or fights.
- Fights produce results that affect hero progression.
- The project explores the domain and gameplay loop more than production-grade infrastructure.

## Core Entities

### Hero

- A player-controlled character.
- Has a portrait, characteristics, and progression state.
- Participates in fights and accumulates results.

### Monster

- An opponent encountered in fights.
- Has a portrait and combat-related properties.

### Fight

- A combat interaction between a hero and a monster.
- Produces a result that is meaningful for progression and history.

### Fight Result

- The outcome of a fight.
- Used for player feedback, statistics, and hero history.

### User Account

- Represents authentication and ownership of game progress.
- Connects a user to one or more gameplay objects.

## Core Domain Loop

1. A user enters the game and authenticates.
2. A hero is created or selected.
3. The hero performs a combat activity.
4. The system resolves the fight and stores the result.
5. The player sees updated hero state and statistics.

## Domain Rules

- A fight must always produce a valid result.
- Hero progression must be derived from gameplay outcomes rather than arbitrary updates.
- Visual identity matters: portraits are part of hero and monster representation.
- Statistics should be explainable from stored fight history.

## Current Domain Decisions

### Hero Combat Characteristics

- The hero currently has Health Points (HP).
- The hero also has Ability.
- At the current MVP stage, Ability is mostly derived from current HP.
- This means a heavily exhausted hero becomes less effective in combat.

### Planned Evolution Of Characteristics

- HP and Ability will become less tightly coupled.
- Additional factors beyond HP will influence Ability.
- Passive and active skills are planned for future iterations.

### Hero Ownership Per User

- At MVP stage, one user can have an unlimited number of heroes.

### Fight Rewards And Penalties

- Rewards: experience and gold.
- Gold will later be spent on consumables and equipment.
- Current hard penalty: permanent death.
- Additional debuffs may be added later.

### Activities Outside Arena

- At the current stage, there are no activities outside arena combat.
- A shop is planned later for consumables and equipment.

## Change Policy

Update this file when business rules, core terms, or gameplay meaning change.