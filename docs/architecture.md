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

## Change Policy

Update this file when project boundaries, module responsibilities, or important flows change.