module Tests

open System
open Xunit

[<Theory>]
[<InlineData(1,42,43)>]
let ``xunit inlinedata hello world`` (a : int, b : int, expected : int) =
    let actual = a + b
    Assert.Equal(expected, actual)