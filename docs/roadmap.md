# Roadmap

## Purpose

This document tracks long-term direction. It is intentionally concise and does not duplicate sprint tasks.

## Current State

MVP in active development. Core hero progression loop is operational: hero creation, monster fights, inventory, shop, item effects in combat, monster item rewards.

## Goal

Build a complete and balanced hero progression loop around combat: meaningful equipment choices, varied combat outcomes, long-term hero growth.

## General Rules

- Keep this file strategic, not operational.
- Put short-term execution tasks in `docs/todo.md`.
- Update this file only when direction changes.

## Strategic Initiatives

### Initiative 1: Equipment Combat Influence

- Description: Equipment should meaningfully affect combat outcomes. Weapons and shields are already integrated; armor system is the next milestone.
- Status: `in-progress`
- Target window: near-term
- Notes: Weapon and shield flow is implemented end-to-end. Next focus: design and implement armor system across backend and frontend while keeping equipment behavior coherent. See `docs/features/weapon-usage.md`.

### Initiative 2: Balanced Economy and Progression

- Description: Gold income, shop prices, and item costs should form a coherent progression curve. Heroes should feel growth over multiple fights.
- Status: `idea`
- Target window: mid-term
- Notes: Depends on having meaningful items to buy (Initiative 1).

### Initiative 3: Combat Depth and Variety

- Description: Introduce hit zone maps per creature type, probabilistic weapon effects (bleeding, poison), and potentially multi-action combat turns.
- Status: `idea`
- Target window: long-term
- Notes: Foundation requires Initiative 1 to be stable.

## Status Model

- `idea`
- `planned`
- `in-progress`
- `done`
- `dropped`

## Change Policy

Update this file when project priorities or major initiatives change.