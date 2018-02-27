# SemVer
A F#/.net SemVer 2.0 implementation draft

Looking for SemVer spec compatible RegExes I have not found an implementation that was correct so I wrote my own.
Features so far:
* Minimum semver includes major.minor.patch parts
* No leading zeros
* Prerelease part should be parsed according to semver.org paragraph 9
* Build part should be parsed according to semver.org paragraph 10

ToDos:
1. Version precedence according to spec
2. Make a lib with NuGet
3. Wrap for comfortable use from C#
