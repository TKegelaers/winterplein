---
name: csharp-xunit
description: >
  Write xUnit unit tests in C# using FluentAssertions, Moq, and the Builder pattern.
  Use this skill whenever the user asks to write, add, or improve unit tests in a C# project,
  or when writing a new handler/service/controller and tests should accompany it.
  Also trigger for questions like "how do I test X", "add tests for Y", "write a test that checks Z".
license: MIT
compatibility: opencode
metadata:
  audience: .net-developers
  framework: xunit
  patterns: unit-testing, fluent-assertions, moq, builder-pattern
---




## What I Do

I help you write clean, idiomatic xUnit tests following the project conventions:
- `[Fact]` tests with FluentAssertions for assertions
- Moq for mocking dependencies
- Builder pattern for creating domain objects
- AAA (Arrange-Act-Assert) structure but never write "Arrange", "Act", "Assert" comments
- Correct naming, namespacing, and file placement

## When to Use Me

Use this skill when:
- Writing unit tests for a new handler, service, or controller
- Adding tests to existing code
- Asking how to test a particular scenario (exception, async, HTTP result type)
- Unsure which assertion or mock pattern to use

## Stack

- **xUnit** — `[Fact]` for single cases; `[Theory]` + `[InlineData]`/`[MemberData]` for parameterised
- **FluentAssertions** — all assertions; never use xUnit's `Assert.*`
- **Moq** — `Mock<IFoo>` with `.Setup()` and `.Verify()`
- **Builder pattern** — test data via builders in `tests/[Project].UnitTests.Common/Builders/`

`Xunit` and `FluentAssertions` are globally imported — no `using` needed for them.

---

## Test class structure

```csharp
// One class per SUT. Filename: [Sut]Tests.cs
// Namespace matches folder: Winterplein.UnitTests.[Layer]

public class FooServiceTests
{
    private readonly Mock<IBarRepository> _repo = new();
    private readonly FooService _sut;

    public FooServiceTests() => _sut = new FooService(_repo.Object);
}
```

Name the SUT field `_sut` when unambiguous, or by role (`_handler`, `_controller`) when the type makes it clear.

---

## Test naming

Pattern: `MethodName_ExpectedOutcome` or `MethodName_ExpectedOutcome_WhenCondition`

```
Handle_ReturnsPlayerDto
Handle_ThrowsArgumentException_WhenGenderInvalid
Handle_CallsRepoWithCorrectGender
GetAll_ReturnsOkWithPlayers
Delete_ReturnsNoContent
```

---

## AAA structure

```csharp
[Fact]
public async Task Handle_ReturnsPlayerDto()
{
    var player = new PlayerBuilder().WithId(5).Build();
    _repo.Setup(r => r.Add(It.IsAny<Name>(), It.IsAny<Gender>())).Returns(player);

    var result = await _sut.Handle(new AddPlayerCommand("John", "Doe", "Male"), CancellationToken.None);

    result.Id.Should().Be(5);
    result.FirstName.Should().Be("John");
}
```

Keep Arrange separate. Act and Assert can share a line for trivial cases.

---

## Moq patterns

```csharp
// Setup sync return
_repo.Setup(r => r.GetAll()).Returns(players);
_repo.Setup(r => r.Add(It.IsAny<Name>(), It.IsAny<Gender>())).Returns(player);

// Setup async return
_sender.Setup(s => s.Send(It.IsAny<MyQuery>(), It.IsAny<CancellationToken>()))
       .ReturnsAsync(result);

// Setup throw
_repo.Setup(r => r.Remove(99)).Throws(new KeyNotFoundException());

// Verify call count + arguments
_repo.Verify(r => r.Remove(42), Times.Once);
_sender.Verify(s => s.Send(It.Is<RemovePlayerCommand>(c => c.Id == 1),
                            It.IsAny<CancellationToken>()), Times.Once);
```

---

## FluentAssertions patterns

```csharp
// Scalar / object equality
result.Id.Should().Be(5);
result.Should().Be(expected);
result.Should().BeEquivalentTo(other);   // deep structural equality

// Collections
result.Should().HaveCount(2);
result.Should().BeEmpty();
result[0].Id.Should().Be(1);

// HTTP result types (controller tests)
var ok = result.Should().BeOfType<OkObjectResult>().Subject;
ok.Value.Should().Be(players);

var created = result.Should().BeOfType<CreatedResult>().Subject;
created.Location.Should().Be("/api/players/5");
created.Value.Should().Be(dto);

result.Should().BeOfType<NoContentResult>();

// Sync exception
var act = () => new Foo(null!);
act.Should().Throw<ArgumentNullException>();

// Async exception
var act = () => _sut.Handle(command, CancellationToken.None);
await act.Should().ThrowAsync<ArgumentException>()
         .WithMessage("*Invalid gender*");   // * = wildcard
```

---

## Builder pattern

Use builders from `tests/[Project].UnitTests.Common/Builders/` for domain objects.
They provide safe defaults so tests only configure what they care about:

```csharp
var player = new PlayerBuilder().Build();                           // defaults
var player = new PlayerBuilder().WithId(5).WithGender(Gender.Female).Build();
var name   = new NameBuilder().WithFirst("Jane").Build();
var match  = new MatchBuilder().WithId(99).Build();
```

When a builder doesn't exist for a new type, create one in `UnitTests.Common/Builders/`
following the same fluent `With*` pattern.

---

## What to cover per unit

1. **Happy path** — correct input, correct output
2. **Correct delegation** — right method called with right arguments (`Verify`)
3. **Error cases** — invalid input throws the right exception type and message
4. **Edge cases** — empty collections, nulls, boundary values where behaviour differs

---

## Test layers

### Domain (`UnitTests/Domain/`)
No mocks. Construct entities directly.

```csharp
[Fact]
public void Player_StoresProperties()
{
    var name = new Name("John", "Doe");
    var player = new Player(1, name, Gender.Female);

    player.Id.Should().Be(1);
    player.Name.Should().Be(name);
    player.Gender.Should().Be(Gender.Female);
}

[Fact]
public void Player_Constructor_ThrowsWhenNameIsNull()
{
    var act = () => new Player(1, null!, Gender.Male);
    act.Should().Throw<ArgumentNullException>();
}
```

### Mapper tests (`UnitTests/Application/`)
No mocks. Call the extension method, assert DTO fields.

```csharp
[Fact]
public void ToDto_MapsAllProperties()
{
    var match = new MatchBuilder().WithId(99).Build();
    var dto = match.ToDto();

    dto.Id.Should().Be(99);
    dto.Team1.Should().BeEquivalentTo(match.Team1.ToDto());
}
```

### Handler tests (`UnitTests/Application/Handlers/`)
Mock the repository interface. Test return value and verify calls.

### Controller tests (`UnitTests/Api/`)
Mock `ISender`. Test HTTP result type, `Location` header, and body value.

```csharp
private readonly Mock<ISender> _sender = new();
private readonly MyController _sut;

public MyControllerTests() => _sut = new MyController(_sender.Object);
```
