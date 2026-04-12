# Domain Overview

## Purpose

Fast Arena MVP is a browser game prototype about short combat runs, hero growth, and lightweight progression. This document covers the product idea, core gameplay loop, and cross-cutting domain rules.

## Product Idea

- The player manages a hero.
- The hero enters short adventures or fights.
- Fights produce results that affect hero progression.
- The project explores the domain and gameplay loop more than production-grade infrastructure.

## User Account

- Represents authentication and ownership of game progress.
- Connects a user to one or more heroes.

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

## Change Policy

Update this file when core product intent, the domain loop, or cross-cutting rules change.
