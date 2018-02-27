#load "SemVer.fsx"

open SemVer

let inline AssertEqual exp inp =
    if not <| (exp = inp) then failwithf "Expected %A but got %A" exp inp

let inline testparse exp inp =
    let semver = parse inp
    match semver with
    | Some s -> AssertEqual exp s
    | None -> failwithf "Failed to parse %s!" inp

let inline expectNone inp =
    let semver = parse inp
    match semver with
    | Some _ -> failwithf "Parsing of %s should fail!" inp
    | None -> ()

let testSemVer = {Major=1; Minor=0;Patch=0;Prerelease=None;Build=None}

let ``Given a simple valid semver string when parsing then the correct semver is returned`` () =
    testparse testSemVer "1.0.0"

let ``Given a valid semver string with a prerelease part when parsing the correct semver is returned`` () =
    testparse {testSemVer with Prerelease=Some [AlphaNumeric "beta"]} "1.0.0-beta"

let ``Given a valid semver string with a build part when parsing the correct semver is returned`` () =
    testparse {testSemVer with Build=Some [Numeric 1]} "1.0.0+1"
    testparse {testSemVer with Build=Some [Numeric 1]} "1.0.0+01"
    testparse {testSemVer with Build=Some [Numeric 1; AlphaNumeric "sha2"]} "1.0.0+1.sha2"

let ``Given a valid semver string with a prerelease and a build part when parsing the correct semver is returned`` () =
    testparse {testSemVer with Prerelease=Some [AlphaNumeric "alpha"];Build=Some [Numeric 1]} "1.0.0-alpha+1"

let ``Given a valid semver string with a multipart prerelease when parsing the correct semver is returned`` () =
    testparse {testSemVer with Prerelease=Some [AlphaNumeric "alpha"; Numeric 1]} "1.0.0-alpha.1"


let ``semver spec#2: Given a version with only a major part must fail`` () =
    expectNone "1"
    expectNone "1."
    expectNone "1-alpha"
    expectNone "1+1"

let ``Given a version with only a major and minor part must fail`` () =
    expectNone "1.0"
    expectNone "1.0."
    expectNone "1.0-alpha"
    expectNone "1.0+1"

let ``Given a semver with leading zeros when parsing it must fail`` () =
    expectNone "01.0.0"
    expectNone "1.00.0"
    expectNone "1.0.00"

let ``Given an input with leading zeros in a numeric prerelease part must fail`` () =
    expectNone "1.0.0-alpha.01"

let ``Given an input with multiple plus chars must fail`` () =
    expectNone "1.0.0+1+2"


``Given a simple valid semver string when parsing then the correct semver is returned`` ()
``Given a valid semver string with a prerelease part when parsing the correct semver is returned`` ()
``Given a valid semver string with a build part when parsing the correct semver is returned`` ()
``Given a valid semver string with a prerelease and a build part when parsing the correct semver is returned`` ()
``Given a valid semver string with a multipart prerelease when parsing the correct semver is returned`` ()

``semver spec#2: Given a version with only a major part must fail``
``Given a version with only a major and minor part must fail`` ()
``Given a semver with leading zeros when parsing it must fail`` ()

``Given an input with leading zeros in a numeric prerelease part must fail`` ()
``Given an input with multiple plus chars must fail`` () 