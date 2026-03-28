# AI Commands

## Purpose

This file lists common commands for local work and validation.

## Run With Docker Compose

From repository root:

```bash
docker compose up --build
```

## Run Backend Locally

From repository root:

```bash
cd backend/src/FastArena.WebHost
dotnet run
```

## Run Frontend Locally

From repository root:

```bash
cd frontend
npm install
npm run serve
```

## Recommended Checks Before Finalizing Changes

- Ensure changed files match the requested scope.
- Ensure no accidental edits outside the task.
- Prefer smallest safe diff that solves the requirement.

## Maintenance

Update commands when runtime or project scripts change.