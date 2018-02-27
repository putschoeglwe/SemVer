
open System
open System.Text.RegularExpressions



let semverRegexPattern = 
    @"^(?<major>0|(?:[1-9][0-9]*))\.(?<minor>0|(?:[1-9][0-9]*))\.(?<patch>0|(?:[1-9][0-9]*))"+
    @"(?:\-(?<prerelease>0|[1-9][0-9]*|[0-9]*[a-zA-Z\-][0-9a-zA-\-]*(?:\.(?:0|[1-9][0-9]*|[0-9]*[a-zA-Z\-][0-9a-zA-Z\-]*))?))?"+
    @"(?:\+(?<build>[0-9a-zA-Z\-]+(?:\.[0-9a-zA-Z\-]+)*))?$"

type Ident =
    | Numeric of int
    | AlphaNumeric of string

type SemVerDesc = {
    Major : int
    Minor : int
    Patch : int
    Prerelease : Ident list option
    Build : Ident list option
}

let getInt (m:Match) (name:string)=
    let group = m.Groups.[name]
    if group.Success then Int32.Parse group.Value
    else failwithf "Error parsing group %s" name

let (|Int|_|) inp =
   match System.Int32.TryParse(inp) with
   | (true,int) -> Some(int)
   | _ -> None

let parseIdent (inp:string) =
    match inp with
    | Int i-> Numeric i
    | _ -> AlphaNumeric inp

let parseIdentList (inp:string) =
    let parts = inp.Split('.')
    parts |> Array.toList |> List.map parseIdent

let getIdentList (m:Match) (name:string) =
    let group = m.Groups.[name]
    if group.Success then Some (parseIdentList group.Value)
    else None

let parse input =
    let reg = Regex(semverRegexPattern)
    let m = reg.Match input
    if m.Success then
        let getIntfromM = getInt m
        let getIdentListFromM = getIdentList m
        Some {Major = getIntfromM "major"; Minor = getIntfromM "minor"; Patch = getIntfromM "patch"; Prerelease = getIdentListFromM "prerelease"; Build = getIdentListFromM "build"}
    else None