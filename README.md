## Features

- [x] Built on .NET 6.0
- [x] Follows Clean Architecture Principles
- [x] Domain Driven Design
- [x] Multi Tenancy Support with Finbuckle
  - [x] Create Tenants with Multi Database / Shared Database Support
  - [x] Activate / Deactivate Tenants on Demand
  - [x] Upgrade Subscription of Tenants - Add More Validity Months to each tenant!
- [x] Supports MySQL, MSSQL, Oracle & PostgreSQL!

<details>
  <summary>Click to See More!</summary>

- [x] Uses Entity Framework Core as DB Abstraction
- [x] Flexible Repository Pattern
- [x] Dapper Integration for Optimal Performance
- [x] Serilog Integration with various Sinks - File, SEQ, Kibana
- [x] OpenAPI - Supports Client Service Generation
- [x] Mapster Integration for Quicker Mapping
- [x] API Versioning
- [x] Response Caching - Distributed Caching + REDIS
- [x] Fluent Validations
- [x] Audit Logging
- [x] Advanced User & Role Based Permission Management
- [x] Code Analysis & StyleCop Integration with Rulesets
- [x] JSON Based Localization with Caching
- [x] Hangfire Support - Secured Dashboard
- [x] File Storage Service
- [x] Test Projects
- [x] JWT & Azure AD Authentication
- [x] MediatR - CQRS
- [x] SignalR Notifications
- [x] & Much More
</details>

## Documentation

Read Documentation related to this Boilerplate here - https://docs.conetso.com/offer-exchange

## Getting Started

> Make sure that your DEV enviroment is setup, [Read the Development Environment Guide](https://github.com/snax4a/offer-exchange-webapi/blob/main/DevSetup.md)

For further steps and details, [Read the Getting Started Guide](https://docs.conetso.com/offer-exchange/getting-started/)

## Important Links & Documentations


## Changelogs

[View Complete Changelogs.](https://github.com/snax4a/offer-exchange-webapi/blob/main/Changelogs.md)

## Commit Message Convention

This website follows [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)

Commit message will be checked using [husky and commit lint](https://theodorusclarence.com/library/husky-commitlint-prettier), you can't commit if not using the proper convention below.

### Format

`<type>(optional scope): <description>`
Example: `feat(pre-event): add speakers section`

### 1. Type

Available types are:

- feat → Changes about addition or removal of a feature. Ex: `feat: add table on landing page`, `feat: remove table from landing page`
- fix → Bug fixing, followed by the bug. Ex: `fix: illustration overflows in mobile view`
- docs → Update documentation (README.md)
- style → Updating style, and not changing any logic in the code (reorder imports, fix whitespace, remove comments)
- chore → Installing new dependencies, or bumping deps
- refactor → Changes in code, same output, but different approach
- ci → Update github workflows, husky
- test → Update testing suite, cypress files
- revert → when reverting commits
- perf → Fixing something regarding performance (deriving state, using memo, callback)
- vercel → Blank commit to trigger vercel deployment. Ex: `vercel: trigger deployment`

### 2. Optional Scope

Labels per page Ex: `feat(pre-event): add date label`

\*If there is no scope needed, you don't need to write it

### 3. Description

Description must fully explain what is being done.

Add BREAKING CHANGE in the description if there is a significant change.

**If there are multiple changes, then commit one by one**

- After colon, there are a single space Ex: `feat: add something`
- When using `fix` type, state the issue Ex: `fix: file size limiter not working`
- Use imperative, dan present tense: "change" not "changed" or "changes"
- Don't use capitals in front of the sentence
- Don't add full stop (.) at the end of the sentence
