module AutoFixture.Community.FSharp

open AutoFixture
open AutoFixture.Kernel

/// Builds F#-specific specimens
type FSharpSpecimenBuilder(fixture: IFixture) =
    interface ISpecimenBuilder with
        member this.Create(request, ctx) =
            match request with
            | List t ->
                [ for _ in 1 .. fixture.RepeatCount -> ctx.Resolve t ]
                |> ReflectiveList.BuildTyped t
            | Union create -> create ctx
            | Unit unitVal -> unitVal
            | _ -> box <| NoSpecimen()

let fsFixture =
    let f = Fixture()

    f.Customizations.Add(FSharpSpecimenBuilder f)
    f :> IFixture

/// Create a random value.
/// In many cases, you don't need to explicitly provide the type argument, which can improve readability
let inline randVal<'a> () = fsFixture.Create<'a>()

/// Create a sequence of random values.
/// In many cases, you don't need to explicitly provide the type argument, which can improve readability
let inline randVals<'a>  () : 'a seq = fsFixture.CreateMany<'a>()

/// Create a sequence of n random values.
/// In many cases, you don't need to explicitly provide the type argument, which can improve readability
let inline randValsN<'a> n : 'a seq = fsFixture.CreateMany<'a>(n)
