# Architecture

## Purpose

This document maps the product domain to the current codebase structure.

## Top-Level Structure

- `backend/` contains the ASP.NET Core backend and data access layers.
- `frontend/` contains the Vue 3 client application.
- `docker-compose.yml` defines the local runtime environment.
- `.env.example` documents required environment variables.

## Backend Overview

The backend solution is located under `backend/src/` and is split into several projects.

### FastArena.Core

- Domain models and business logic.
- Core services, interfaces, and exceptions.

### FastArena.Dal

- Entity Framework Core data access.
- Database context, mappings, and storage implementations.
- Schema is defined in `ApplicationContext`. Base data is seeded at runtime by `FastArena.WebHost` seeders, not by EF `HasData`.
- Item effect definitions are stored in DAL and linked to items (item-owned effect records).

### FastArena.WebApi

- API-facing DTOs, controllers, and mapping profiles.
- Converts internal models into HTTP responses.

### FastArena.WebHost

- Application startup and runtime configuration.
- Hosts the web application.
- `Services/Seeders/` contains runtime seeders that populate base data on first startup: portraits, monsters, and items.
- `wwwroot/assets/` serves static files: `portraits/`, `creatures/`, `items/`.

### FastArena.ApiClientGenerator

- Generates API client code used by the frontend.

## Frontend Overview

The frontend is a Vue 3 application with TypeScript, Pinia, and Vue Router.

### Main Areas

- `src/pages/` contains route-level screens.
- `src/components/` contains reusable UI and feature components.
- `src/stores/` contains Pinia stores for client state.
- `src/services/` contains client-side application services.
- `src/api/` contains generated or wrapped API clients.
- `src/router/` contains route definitions.

## Data Flow

1. The frontend triggers an action from a page, component, or store.
2. A client service or store calls the API layer.
3. The backend controller receives the request.
4. Backend services and DAL resolve domain logic and persistence.
5. The backend returns DTOs to the frontend.
6. The frontend updates state and UI.

## Current Boundaries

- Domain rules should stay primarily in backend core logic.
- Frontend should reflect server state and provide interaction flow.
- Mapping between DAL entities, domain models, and API DTOs is a critical seam.

## Hotspots

- Fight resolution flow.
- Hero creation and portrait selection.
- Mapping depth and nested response models.
- Statistics derived from stored fight data.
- Shop transaction flow: sell/buy selections confirmed atomically via `POST /api/shop/transaction`.
- Staged effect system rollout:
	- implemented: effect definition persistence and seed data,
	- pending: runtime effect execution pipeline in fight lifecycle.

## Implementation Notes: Effect System

Current technical status:

- Effect definitions are persisted as item-owned records in DAL and are exposed through API item DTO mapping.
- Seed includes potion effect definitions for healing, ability override, and strike power bonus.
- Current persisted effect parameters include:
	- type,
	- duration rounds,
	- magnitude,
	- min/max value bounds,
	- chance percent,
	- condition type,
	- target type,
	- priority,
	- optional next effect definition id.

Scope boundary for current phase:

- Implemented: definition model, storage mapping, and seed data.
- Not implemented yet: runtime active-effect lifecycle, event/phase dispatch, per-effect handlers, and stacking execution rules.

## Planned Runtime Contract: Fight Effect Hooks

This section describes implementation-level direction for runtime effect execution.

### Goal

- Use one unified fight-effect execution scheme for all effect types.
- Keep effect-specific behavior inside dedicated handlers.
- Keep fight service focused on phase progression, not on effect-specific branching.

### Hook-Based Phase Contract

Runtime effect processing is planned around fixed hooks in fight lifecycle:

1. `RoundStart`
	- normalize active effects,
	- apply stack/merge rules for equal effect types,
	- decrement/update duration lifecycle.
2. `BeforeInitiative`
	- apply pre-roll effect influence (for example heal or characteristic override activation).
3. `AfterInitiativeRoll`
	- react to roll results before strike confirmation.
4. `OnStrikeConfirmed`
	- process defense-window effects on confirmed incoming strike.
5. `BeforeDamageCommit`
	- apply strike power and pre-commit damage adjustments.
6. `AfterDamageCommit`
	- apply post-hit effects and state updates tied to committed damage.
7. `RoundProjection`
	- build final effective round values for API response after all recalculations.
8. `Finalize`
	- process fight-end effects when result is finalized.

### Handler Behavior Rules

- Each effect handler participates in the same hook contract.
- Hooks that are irrelevant for a handler are expected to be no-op.
- A handler may be active in multiple hooks during the same round.
- Deterministic execution order is required on every hook pass.

### Stacking And Merge Direction

- Equal effect types are composed by effect-specific stack policy.
- Composition may preserve multiple active instances or merge into one aggregated active effect.
- Merge behavior (for example summing magnitude) belongs to the effect handler/strategy for that effect type.

## Change Policy

Update this file when project boundaries, module responsibilities, or important flows change.