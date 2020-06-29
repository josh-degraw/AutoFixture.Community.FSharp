namespace AutoFixture.Extensions

open System
open System.Reflection
open AutoFixture.Kernel
open FSharp.Reflection

[<AutoOpen>]
module internal Utils =

    // Copied from https://blog.nikosbaxevanis.com/2016/04/03/creating-fsharp-lists-with-autofixture/
    [<Sealed>]
    type ReflectiveList =

        static member Build<'a>(args: obj list) =
            [ for a in args do
                yield a :?> 'a ]

        static member BuildTyped (t: Type) args =
            let buildMethod =
                typedefof<ReflectiveList>.GetMethod("Build", BindingFlags.Static ||| BindingFlags.NonPublic)
            let buildMethod = buildMethod.MakeGenericMethod(t)

            buildMethod.Invoke(null, [| args |])

    let (|List|_|) candidate =
        match candidate |> box with
        | :? Type as t when t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<list<_>> ->
            let argType = t.GetGenericArguments().[0]
            List(argType) |> Some
        | _ -> None

    let rand = Random()

    let getRandomElement arr =
        let length = Array.length arr
        let index = rand.Next(0, length)

        Array.get arr index

    let (|Union|_|) candidate =
        match candidate |> box with
        | :? Type as t when FSharpType.IsUnion(t) ->
            let cases = FSharpType.GetUnionCases(t)
            let case = getRandomElement cases

            let args = case.GetFields() |> Array.map (fun f -> f.PropertyType)

            let create (ctx: ISpecimenContext) =
                let args = args |> Array.map ctx.Resolve
                FSharpValue.MakeUnion(case, args)

            Some <| Union(create)
        | _ -> None

    let (|Unit|_|) candidate =
        match candidate |> box with
        | :? Type as t when t = typedefof<unit> ->
            ()
            |> box
            |> Some
        | _ -> None
