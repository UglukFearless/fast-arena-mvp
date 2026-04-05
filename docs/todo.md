# Todo

## Purpose

This document tracks short-horizon tasks. It should stay compact and operational.

## Current Priorities

### Documentation

- [ ] Finalize the structure of project documentation.
- [ ] Fill `docs/domain.md` with concrete business rules.
- [ ] Fill `docs/architecture.md` with codebase-specific examples.

### Next Goal: Hero Inventory

- [x] Rework item seed. All items seeded via runtime `ItemSeeder`; images served from backend `wwwroot/assets/items/`.
- [x] Inventory with items is integrated into `HeroInfo`.
- [ ] Define and implement item acquisition path for heroes.

## Task Format

When adding new tasks, prefer this structure:

- `[ ] task`
- optional owner
- optional note or link to related file/feature

## Change Policy

Update this file frequently. Remove completed items when they no longer matter, or move major completed work into changelog or roadmap notes.