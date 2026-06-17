# KOAla Service Project Layout

Reference layout derived from `KOAla.Opvangvoorzieningen`. Use to set up a new service or refactor an existing one to match the standard structure.

Replace `{Service}` with the service name (e.g. `Facturatie`, `Klanten`, `Opvangvoorzieningen`).

---

## Solution Projects

```
KOAla.{Service}.slnx                         Solution file (CLI-managed — never hand-edit)

# Core layers (dependency order: Domain → Application.IO → Application → Infrastructure)
KOAla.{Service}.Domain                        Pure domain model — no external service deps
KOAla.{Service}.Application.IO               DTOs, commands, queries, domain events (shared with callers)
KOAla.{Service}.Application                  CQRS handlers, mappers, ports, event publishers
KOAla.{Service}.Infrastructure               EF DbContext(s), repositories, caching

# Adapters
KOAla.{Service}.Providers.{ExternalSystem}   One project per external integration
KOAla.{Service}.Providers.Rebus              Rebus message bus wiring (subscribes/publishes)
KOAla.{Service}.Providers.Hangfire           Hangfire job scheduling wiring (optional)

# Runnable hosts
KOAla.{Service}.WebApi                       ASP.NET Core host — controllers, startup, IoC config
KOAla.{Service}.Database                     DbUp migration runner (standalone console app)
KOAla.{Service}.Synchronisatie               ETL/sync standalone runner (optional)

# Tests
KOAla.{Service}.Domain.UnitTests
KOAla.{Service}.Application.UnitTests
KOAla.{Service}.Infrastructure.UnitTests
KOAla.{Service}.WebApi.UnitTests
KOAla.{Service}.Providers.{ExternalSystem}.UnitTests
KOAla.{Service}.Synchronisatie.UnitTests     (if Synchronisatie exists)
KOAla.{Service}.Common.UnitTests             Shared test helpers/fixtures
KOAla.{Service}.Common.IntegrationTests
KOAla.{Service}.IntegrationTests
```

---

## Dependency Graph

```
Domain
  └── KOAla.Common.Domain
  └── KOAla.Common.Utils

Application.IO
  └── KOAla.Common.Application.IO            (Rebus shared events, MediatR)

Application
  └── Application.IO
  └── Domain
  └── KOAla.Common.Utils
  └── [MediatR, FluentValidation]

Infrastructure
  └── Application
  └── Domain
  └── KOAla.Common.Infrastructure
  └── [EF Core Relational, LazyCache]

Providers.{ExternalSystem}
  └── Application.IO
  └── Application
  └── [Azure.Messaging.ServiceBus / external SDK]

Providers.Rebus
  └── Application
  └── Providers.{ExternalSystem}             (needs domain events from provider)
  └── [MediatR, Ons.SharedCode.Rebus]

Providers.Hangfire
  └── Application.IO
  └── KOAla.Common.Hangfire
  └── [Rebus]

WebApi
  └── Application.IO
  └── Application
  └── Infrastructure
  └── Providers.*  (all provider projects)
  └── KOAla.Common.ServiceBus
  └── KOAla.Common.WebApi
  └── [EF Core SqlServer, LazyCache.AspNetCore, Ella.Application]

Database
  └── KOAla.Common.DbUp                      (standalone — no service deps)

Synchronisatie
  └── Application.IO (or Application)
  └── [whatever ETL deps needed]
```

**Rules:**

- `Domain` has zero deps on `Application` or `Infrastructure` — never violate.
- `Application.IO` has no `Domain` reference — it is a pure contracts layer.
- `WebApi` references all providers; providers never reference `WebApi`.
- `Database` references only `KOAla.Common.DbUp` — it is fully standalone.

---

## Internal Folder Structure

### `Domain/`

```
Domain/
  IOpvangvoorziening.cs              Shared aggregate interface (if the domain has sub-types)
  {SharedValueObject}.cs             Root-level value objects (Email, Adres, Telefoon, etc.)

  {SubType}/                         One folder per domain sub-type (BKO, KDV, DVO, LD, VOB)
    {Aggregate}.cs                   Aggregate root
    {Aggregate}Nr.cs                 Typed ID value object
    {ChildEntity}.cs
    {ChildEntityNr}.cs
    {ValueObject}.cs

  Communicaties/                     Cross-cutting domain concepts (if applicable)
  DigitaalPlatform/                  External register domain types (if applicable)
```

### `Application.IO/`

```
Application.IO/
  IAmApplicationIO.cs                Marker interface

  Commands/
    {SubType}/
      Create{Aggregate}Command.cs
      Update{Aggregate}Command.cs
      Delete{Aggregate}Command.cs

  Queries/
    {SubType}/
      Get{Aggregate}Query.cs
      GetAll{Aggregates}Query.cs

  DTOs/
    {SubType}/
      {Aggregate}Dto.cs
      {ChildEntity}Dto.cs

  DomainEvents/
    {SubType}/
      {Aggregate}ChangedDomainEvent.cs
      {Aggregate}HoofdlocatieChangedDomainEvent.cs
      {Aggregate}OpvanglocatieChangedDomainEvent.cs

  SearchModels/
    {Aggregate}SearchModel.cs

  Services/
    {SubType}/
      I{Aggregate}Service.cs
      {Aggregate}Service.cs

  {Service}FilterModel.cs
  {Service}ReadModel.cs
  {Service}WriteModel.cs
```

### `Application/`

```
Application/
  IAmApplication.cs                  Marker interface

  CommandHandlers/
    {SubType}/
      Create{Aggregate}/
        Create{Aggregate}CommandHandler.cs
        Create{Aggregate}CommandValidator.cs
      Update{Aggregate}/
        Update{Aggregate}CommandHandler.cs
        Update{Aggregate}CommandValidator.cs
      Delete{Aggregate}/
        Delete{Aggregate}CommandHandler.cs

  QueryHandlers/
    {SubType}/
      Get{Aggregate}QueryHandler.cs
      GetAll{Aggregates}QueryHandler.cs

  EventPublishers/
    {SubType}/
      I{Aggregate}EventPublisher.cs
      {Aggregate}EventPublisher.cs

  Ports/
    I{Aggregate}Repository.cs
    I{ExternalSystem}Loader.cs
    I{ExternalSystem}Syncer.cs

  Mappers/
    {SubType}/
      {Aggregate}DtoMapper.cs
      {ChildEntity}DtoMapper.cs

  Providers/
    ICacheManager.cs
    CacheKeys.cs
    I{Service}Provider.cs
    {Service}Provider.cs

  Commands/                          Internal application-only commands (not in .IO)
    {InternalCommand}.cs

  {Service}TeamNrHelpers.cs          (optional utility)
```

### `Infrastructure/`

```
Infrastructure/
  {Service}DbContext.cs              Primary EF DbContext
  {SubSystem}DbContext.cs            Secondary DbContext if needed (e.g. Communicaties)

  {Aggregate}Repository.cs           Root-level cross-type repository (optional)

  Common/
    Models/
      BaseDataModel.cs
      {SharedEntity}DataModel.cs
    Mappers/
      {SharedEntity}DataMapper.cs

  {SubType}/                         One folder per domain sub-type
    {Aggregate}Repository.cs
    Models/
      {Aggregate}DataModel.cs
      {ChildEntity}DataModel.cs
      Search/
        {Aggregate}SearchModel.cs
    Mappers/
      {Aggregate}DataMapper.cs
      {ChildEntity}DataMapper.cs

  Caching/
    CacheConfig.cs
    CacheManager.cs

  DigitaalPlatform/                  Infrastructure support for external register (if applicable)
    Models/
    Mappers/
    Repositories/
    Context/
```

### `Providers.{ExternalSystem}/`

```
Providers.{ExternalSystem}/
  {ExternalSystem}Provider.cs
  {ExternalSystem}Config.cs
  Models/
    {ExternalEntity}.cs
  Mappers/
    {ExternalEntity}Mapper.cs
  Repositories/ (or Clients/)
    {ExternalSystem}Repository.cs
```

### `Providers.Rebus/`

```
Providers.Rebus/
  RebusConfig.cs (or registered in WebApi/Configuration/RebusConfig.cs)
  Consumers/ (or Handlers/)
    {Event}Handler.cs
  Publishers/
    {Event}Publisher.cs
```

### `WebApi/`

```
WebApi/
  Program.cs
  Startup.cs

  Configuration/
    IocConfig.cs
    RebusConfig.cs
    AzureServiceBusConfig.cs (or similar)

  Controllers/
    {SubType}Controller.cs            One controller per aggregate type
    {CrossCutting}Controller.cs       e.g. OpvanglocatiesController, AdminController

  Handlers/                           Rebus/event message handlers wired to Application
    {Event}ChangedDomainEventHandler.cs
    Mappers/
      {Event}Mapper.cs

  Ella/                               Integration-specific sub-area (if applicable)
    DTOs/
    {SubArea}Mapper.cs
    {SubArea}Query.cs

  Properties/
    launchSettings.json
  appsettings.json
```

### `Database/`

```
Database/
  Program.cs
  Migrations/
    Scripts/
      {001_yyyymmdd}_{description}.sql    DbUp idempotent scripts
```

---

## Key Conventions

### Marker Interfaces

Every service defines:

- `IAmApplication` in `Application/` — used by IoC to scan the assembly
- `IAmApplicationIO` in `Application.IO/` — used by callers to reference contracts

### Command/Query Naming

| Pattern                    | Example                                            |
| -------------------------- | -------------------------------------------------- |
| `Create{Entity}Command`    | `CreateBuitenschoolseKinderopvangCommand`          |
| `Update{Entity}Command`    | `UpdateBuitenschoolseKinderopvangCommand`          |
| `Delete{Entity}Command`    | `DeleteReservatieperiodeCommand`                   |
| `Get{Entity}Query`         | `GetBuitenschoolseKinderopvangQuery`               |
| `GetAll{Entities}Query`    | `GetAllBuitenschoolseKinderopvangenQuery`          |
| `{Entity}CommandHandler`   | `CreateBuitenschoolseKinderopvangCommandHandler`   |
| `{Entity}CommandValidator` | `CreateBuitenschoolseKinderopvangCommandValidator` |

### Repository Port Naming

```csharp
// Application/Ports/
public interface I{Aggregate}Repository { ... }        // CRUD port
public interface I{External}Loader { ... }              // read-only external port
public interface I{External}Syncer { ... }              // write external port
```

### EventPublisher Pattern

```csharp
// Application/EventPublishers/{SubType}/
public interface I{Aggregate}EventPublisher
{
    Task Publish{Aggregate}ChangedEventAsync({Aggregate} aggregate);
}

// Implementation registered in Providers.Rebus
public class {Aggregate}EventPublisher : I{Aggregate}EventPublisher { ... }
```

### Domain Sub-Type Hierarchy

When a service has multiple domain sub-types (e.g. BKO, KDV, DVO), mirror the sub-type folder in **every** project layer:

```
Domain/BKO/         Application/CommandHandlers/BKO/       Infrastructure/BKO/
Domain/KDV/         Application/CommandHandlers/KDV/       Infrastructure/KDV/
Domain/DVO/         Application/CommandHandlers/DVO/       Infrastructure/DVO/
```

All sub-types implement a shared `I{ServiceEntity}` interface defined at the root of `Domain/`.

### Value Object Naming

| Concept              | Naming                                                                |
| -------------------- | --------------------------------------------------------------------- |
| Typed ID             | `{Aggregate}Nr` (e.g. `KinderdagverblijfNr`)                          |
| Typed ID child       | `{ChildEntity}Nr` (e.g. `LeefgroepNr`)                                |
| External identifier  | Descriptive name (e.g. `OpvangVlaanderenNummer`, `Vergunningsnummer`) |
| Shared value objects | Root of `Domain/` (e.g. `Adres`, `Email`, `Telefoon`, `Coordinaten`)  |

---

## Test Project Setup

All test projects use **xUnit + Moq + FluentAssertions**.

```
{Project}.UnitTests/
  Usings.cs                          Global usings (xunit, Moq, FluentAssertions)
  {Aggregate}Tests.cs                Tests named after the class under test
  {CommandHandler}Tests.cs
```

Test class pattern — constructor-based setup, no base class:

```csharp
public class Create{Entity}CommandHandlerTests
{
    private readonly Mock<I{Entity}Repository> _repository = new();
    private readonly Create{Entity}CommandHandler _sut;

    public Create{Entity}CommandHandlerTests()
    {
        _sut = new Create{Entity}CommandHandler(_repository.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_Creates{Entity}()
    {
        // Arrange / Act / Assert
    }
}
```

---

## Pre-PR Checklist

```powershell
dotnet build 02.Services/KOAla.{Service}/KOAla.{Service}.slnx
dotnet test 02.Services/KOAla.{Service}/KOAla.{Service}.slnx -- --filter-not-trait Category=Integration
dotnet format 02.Services/KOAla.{Service}/KOAla.{Service}.slnx
```

Verify: no new build warnings in changed projects, all unit tests green, format produces no diff in unrelated files.

---

## Hard Constraints

| Rule                           | Detail                                                                                                         |
| ------------------------------ | -------------------------------------------------------------------------------------------------------------- |
| No EF migrations               | Use DbUp SQL scripts only. Never run `dotnet ef migrations`.                                                   |
| Domain isolation               | `Domain` has zero deps on `Application`, `Infrastructure`, or any provider.                                    |
| No `WebApi` ref from providers | Providers only know about `Application` and `Application.IO`.                                                  |
| `Database` is standalone       | Only references `KOAla.Common.DbUp` — no application logic.                                                    |
| `Synchronisatie` is standalone | Not hosted in `WebApi` — run independently for ETL jobs.                                                       |
| Idempotent SQL                 | All `Database/Migrations/` scripts must be safe to re-run: `SET XACT_ABORT ON; BEGIN TRANSACTION; ... COMMIT;` |
| Env var prefix                 | Each service uses its own prefix: `KOALA_{SERVICE}_` (e.g. `KOALA_OPVANGVOORZIENINGEN_`).                      |
