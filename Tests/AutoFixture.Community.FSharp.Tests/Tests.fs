module AutoFixture.Community.FSharp.Tests

open Xunit
open FsUnit.Xunit

let throwAny = throw typeof<System.Exception>

[<Fact>]
let ``Can create an F# list without throwing an exception`` () =
    let make () = randVal<int list>()
    make
    |> should not' throwAny

[<Fact>]
let ``Can create a non-empty F# list`` () =
    let list = randVal<int list>()
    list
    |> List.length
    |> should be (greaterThan 0)

type TestRecordType = {
    MyInt : int
    MyString: string
}

[<Fact>]
let ``Can create a simple record type``() =
    let create () = randVal<TestRecordType>()

    create
    |> should not' throwAny


type ComplexTestRecordType = {
    MyInt : int
    MyString: string
    MyList: string list
}

[<Fact>]
let ``Can create a complex record type``() =
    let item = randVal<ComplexTestRecordType>()

    item.MyList
    |> List.length
    |> should be (greaterThan 0)


type ComplexTestRecordTypeWithDuMembers = {
    MyInt : int
    MyString: string
    MyList: string list
    MyOption : string option
    MyOtherOption: int option
}

[<Fact>]
let ``Can create a complex record type with discriminated union members``() =
    let create() = randVal<ComplexTestRecordTypeWithDuMembers>()

    create
    |> should not' throwAny

[<Fact>]
let ``Can create a tuple``() =
    let make () = randVal<int * string>()

    make
    |> should not' throwAny


[<Fact>]
let ``Can create an option without error``() =
    let make () = randVal<int option>()

    make
    |> should not' throwAny


[<Fact>]
let ``Can create an option``() =
    let item = randVal<int option>()

    match item with
    | Some v ->
        v
        |> should be (greaterThan 0)
    | None -> ()

type SampleUnionNoMembers =
| FirstMember
| SecondMember

[<Fact>]
let ``Can create members of a union type without any data``() =
    let create () = randVal<SampleUnionNoMembers>()

    create
    |> should not' throwAny

[<Fact>]
let ``Can create values while excluding certain values`` () =
    let create () = randValExcept FirstMember

    for _ = 0 to 25 do
        create()
        |> should not' (equal FirstMember)


[<Fact>]
let ``Can create sequences of values while excluding certain values`` () =
    let create () = randValsNExcept 10 FirstMember

    for _ = 0 to 25 do
        create()
        |> should not' (contain FirstMember)