module AutoFixture.Community.FSharp

open AutoFixture
open AutoFixture.Kernel

/// Builds F#-specific specimens
type FSharpSpecimenBuilder (fixture: IFixture) =
    interface ISpecimenBuilder with
        member this.Create (request, ctx) =
            match request with
            | List t ->
                [ for _ in 1 .. fixture.RepeatCount -> ctx.Resolve t ]
                |> ReflectiveList.BuildTyped t
            | Union create -> create ctx
            | Unit unitVal -> unitVal
            | _ -> box <| NoSpecimen ()

let fsFixture =
    let f = Fixture () :> IFixture
    f.Customizations.Add (FSharpSpecimenBuilder f)
    f

/// Create a random value.
let inline randVal<'a> () = fsFixture.Create<'a> ()

/// Create a sequence of random values.
let inline randVals<'a> () : 'a seq = fsFixture.CreateMany<'a> ()

/// Create a sequence of n random values.
let inline randValsN<'a> n : 'a seq = fsFixture.CreateMany<'a> n

/// Create a random value, ensuring that the provided predicate is not true for the generated value.
let inline randValExceptWhere<'a> fn =
    let mutable value = randVal<'a> ()

    while fn value do
        value <- randVal<'a> ()

    value

/// Create a random value, ensuring that the provided value is never what is generated.
let inline randValExcept<'a when 'a: equality> x = randValExceptWhere ((=) x)

/// Create an infinite sequence of random values, ensuring that the provided predicate is not true for any of the generated values.
let inline randValsExceptWhere<'a> fn =
    Seq.initInfinite (fun _ -> randValExceptWhere<'a> fn)

/// Create an infinite sequence of random values, ensuring that the provided value is never what is generated.
let inline randValsExcept<'a when 'a: equality> x = randValsExceptWhere<'a> ((=) x)

/// Create a finite sequence of n random values, ensuring that the provided predicate is not true for any of the generated values.
let inline randValsNExceptWhere<'a> n fn =
    randValsExceptWhere<'a> fn |> Seq.take n

/// Create a finite sequence of n random values, ensuring that the provided value is never what is generated.
let inline randValsNExcept<'a when 'a: equality> n x = randValsNExceptWhere<'a> n ((=) x)
