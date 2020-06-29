module AutoFixture.Extensions.FSharp.Xunit.Tests

open AutoFixture.Extensions.FSharp
open FsUnit.Xunit
open Xunit

type SimpleRecordType = {
    MyInt : int
    MyString: string
}
let throwAny = throw typeof<System.Exception>

[<Theory; AutoDataFSharp>]
let ``Can populate test function with a single record type`` (record: SimpleRecordType) =
    record
    |> should not' (be Null)
    
type ComplexRecordType = {
    MyInt : int
    MyString: string
    MyOption: int option
    MyOtherRecord: SimpleRecordType
    MyTupleField: string * float
}

[<Theory; AutoDataFSharp>]
let ``Can populate test function with a complex record type`` (record: ComplexRecordType) =
    record
    |> should not' (be Null)
    
[<Theory; AutoDataFSharp>]
let ``Can populate test function with a tuple argument`` (myInt: int, myString: string) =
    myInt |> should be (greaterThan 0)
    myString |> should not' (be NullOrEmptyString)
    
        
[<Theory; AutoDataFSharp>]
let ``Can populate test function with a tuple argument including a record type`` (myInt: int, myRecord: ComplexRecordType) =
    myInt |> should be (greaterThan 0)
    myRecord |> should not' (be Null)
    

[<Theory; AutoDataFSharp>]
let ``Can populate a test function with curried arguments`` (myInt: int) (myString: string) =
    myInt |> should be (greaterThan 0)
    myString |> should not' (be NullOrEmptyString)

[<Theory; AutoDataFSharp>]
let ``Can populate test function with curried arguments including a record type`` (myInt: int) (myRecord: ComplexRecordType) =
    myInt |> should be (greaterThan 0)
    myRecord |> should not' (be Null)


[<Theory; AutoDataFSharp>]
let ``Can populate test function with curried arguments including a record type and tuples``
    (myInt: int)
    (myRecord: ComplexRecordType)
    (myTuple: string option * float)
    =
    myInt |> should be (greaterThan 0)
    myRecord |> should not' (be Null)
    myTuple |> should not' (be Null)
    
    let (_, _)= myTuple
    ()
   
[<Theory>]
[<InlineAutoDataFSharp(2)>]
let ``Can populate some items from InlineAutoData attribute`` (number: int) (dto: ComplexRecordType) =
    number |> should equal 2
    dto.MyString |> should not' (be NullOrEmptyString)