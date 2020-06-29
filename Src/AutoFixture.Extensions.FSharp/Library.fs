module AutoFixture.Extensions.FSharp    
open AutoFixture
open AutoFixture.Kernel

type FSharpSpecimenBuilder(fixture: IFixture) =
    interface ISpecimenBuilder with 
        member this.Create(request, ctx) =
            match request with
            | List t ->
                [ for _ in 1..fixture.RepeatCount -> ctx.Resolve t ]
                |> ReflectiveList.BuildTyped t
            | Union create -> create ctx
            | Unit unitVal -> unitVal
            | _ -> box <| NoSpecimen()
            
type FSharpCustomization() =        
    interface ICustomization with
        member this.Customize fixture =                
            FSharpSpecimenBuilder fixture
            |> fixture.Customizations.Add 
    
let fsFixture =
    let f = Fixture()
    let c = FSharpCustomization() :> ICustomization
    c.Customize f
    f :> IFixture

/// Create a random value. 
/// In many cases, you don't need to explicitly provide the type argument, which can improve readability 
let inline randVal<'a> () = fsFixture.Create<'a>()

/// Create a sequence of random values.
/// In many cases, you don't need to explicitly provide the type argument, which can improve readability 
let inline randVals<'a> () = fsFixture.CreateMany<'a>()

/// Create a sequence of n random values. 
/// In many cases, you don't need to explicitly provide the type argument, which can improve readability 
let inline randValsN<'a> n = fsFixture.CreateMany<'a>(n)