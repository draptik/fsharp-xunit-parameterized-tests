module Tests

open System
open Xunit

(*
    Different ways to use Xunit's MemberData attribute with F#.
    
    Sources used:
        - [Using xUnit Theory Member Data with F#](https://www.jessesquire.com/articles/2018/02/17/xunit-memberdata-with-fsharp/)
        - [In F# how do you pass a collection to xUnit's InlineData attribute](https://stackoverflow.com/questions/35026735/in-f-how-do-you-pass-a-collection-to-xunits-inlinedata-attribute)
*)

// InlineData example
[<Theory>]
[<InlineData(1,42,43)>]
let ``xunit inlinedata hello world`` (a : int) (b : int) (expected : int) =
    let actual = a + b
    Assert.Equal(expected, actual)

// Return signature: obj [] list
let sampleNumbers : obj [] list =
    [
        [| 1 |]
        [| 2 |]
        [| 3 |]
    ]

// Theory with MemberData: simple example
[<Theory>]
[<MemberData(nameof(sampleNumbers))>]
let ``xunit memberData hello world`` number =
    Assert.True(number > 0)

// Simpsons
type Person = { Name: string; Incidents: int; Age: int }
let lisa = { Name = "Lisa"; Incidents = 0; Age = 6 }
let marge = { Name = "Marge"; Incidents = 0; Age = 39 }
let homer = { Name = "Homer"; Incidents = 10; Age = 42 }
let bart = { Name = "Bart"; Incidents = 42; Age = 8 }
    
let samplePeople =
    [
        [| homer |]
        [| marge |]
        [| lisa |]
        [| bart |]
    ]

//let samplePeople' =
//    [homer;marge;lisa;bart]
//    |> List.map (fun x -> [|x|])
    
[<Theory>]
[<MemberData(nameof(samplePeople))>]
let ``xunit memberData with single type`` person =
    Assert.True(person.Age > 0)
    
// When mixing different data types (here: Person and string) ensure that the returned collection is obj[]    
let sampleDataWithExpected : obj[] seq =
    seq {
        yield [| homer; "Homer" |]
        yield [| marge; "Marge" |]
    }

[<Theory>]
[<MemberData(nameof(sampleDataWithExpected))>]
let ``xunit memberData with different types and return signature seq`` person name =
    Assert.Equal(name, person.Name)
    
// No return signature -> boxing
// Only the first element of the first collection has to be boxed    
let sampleData2WithExpected =
    seq {
        yield [| box homer; "Homer" |]
        yield [| marge; "Marge" |]
    }

[<Theory>]
[<MemberData(nameof(sampleData2WithExpected))>]
let ``xunit memberData with different types and no return signature and boxing`` person name =
    Assert.Equal(name, person.Name)

// No return signature -> casting to object
// Only the first element of the first collection has to be casted    
let sampleData3WithExpected =
    seq {
        yield [| homer :> Object; "Homer" |]
        yield [| marge; "Marge" |]
    }

[<Theory>]
[<MemberData(nameof(sampleData3WithExpected))>]
let ``xunit memberData with different types and no return signature and upcasting`` person name =
    Assert.Equal(name, person.Name)

// Shortest variant
let samplePeopleWithResult : obj[] list =
    [
        [| homer; "Homer" |]
        [| marge; "Marge" |]
        [| lisa; "Lisa" |]
        [| bart; "Bart" |]
    ]
    
[<Theory>]
[<MemberData(nameof(samplePeopleWithResult))>]
let ``xunit memberData with different types and return signature list`` person name =
    Assert.Equal(name, person.Name)

type Somebody = { Name : string }
let samplesTLDR : obj[] list =
    [
        [| { Somebody.Name = "Homer" }; "Homer" |]
        [| { Somebody.Name = "Marge" }; "Marge" |]
    ]
    
[<Theory>]
[<MemberData(nameof(samplesTLDR))>]
let ``test TLDR`` someBody expected =
    Assert.Equal(expected, someBody.Name)
    