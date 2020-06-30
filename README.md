# AutoFixture.Community.FSharp

![Build](https://github.com/josh-degraw/AutoFixture.Community.FSharp/workflows/Build/badge.svg)

| Package | Version |
| ------- | ------- |
| AutoFixture.Community.FSharp       | [![AutoFixture.Community.FSharp](https://img.shields.io/nuget/v/AutoFixture.Community.FSharp)](https://www.nuget.org/packages/AutoFixture.Community.FSharp)            |
| AutoFixture.Community.FSharp.Xunit | [![AutoFixture.Community.FSharp.Xunit](https://img.shields.io/nuget/v/AutoFixture.Community.FSharp.Xunit)](https://www.nuget.org/packages/AutoFixture.Community.FSharp.Xunit) |

This is a small library that builds on top of [AutoFixture](https://github.com/AutoFixture/AutoFixture) to add support for F#-specific types.
Specifically, the important additions that are't currently handled well with vanilla `AutoFixture` are:

1. Discriminated Unions
2. F# lists

Everything else seems to work pretty well out of the box.

## Useful bits:

### `FSharpSpecimenBuilder`:

Can be inserted onto any existing `IFixture` by doing the following

```fsharp
let fixture = Fixture()
//...

fixture.Customizations.Add(FSharpSpecimenBuilder(fixture))
```

### `fsFixture`

A pre-made fixture with only the F# features applied.

### `randVal` and friends

Helper functions for creating random data inline, e.g.:

```fsharp
type MyDto = 
  { Foo : int
    Bar : string
    Baz : DateTime }
    
[<Fact>]
let ``My test method when Foo is 5`` () = 
  let myDto = 
    { Foo = 5
      bar = randVal()
      Baz = randVal() }
  // Do assertions
```

Thanks to F#'s type system, _most_ of the time you can omit the type arguments.
Similar helper functions include:
- `randVal<'a>()` : create a single random value
- `randVals<'a>()` : create an `'a seq`
- `randValsN<'a> n` : create an `'a seq` that is `n` elements long

## AutoFixture.Community.FSharp.Xunit

This package defines just two attributes:

1. `AutoDataFSharpAttribute`
2. `InlineAutoDataFSharpAttribute`

Which respectively correspond to the `AutoDatAttribute` and `InlineAutoDataAttribute` from [AutoFixture.Xunit2](https://blog.ploeh.dk/2010/10/08/AutoDataTheorieswithAutoFixture/)
