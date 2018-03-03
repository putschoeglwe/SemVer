using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SemVer
{
    public class SemVerParser
    {
        private const string SemverRegexPattern =
            @"^(?<major>0|(?:[1-9][0-9]*))\.(?<minor>0|(?:[1-9][0-9]*))\.(?<patch>0|(?:[1-9][0-9]*))" +
            @"(?:\-(?<prerelease>0|[1-9][0-9]*|[0-9]*[a-zA-Z\-][0-9a-zA-\-]*(?:\.(?:0|[1-9][0-9]*|[0-9]*[a-zA-Z\-][0-9a-zA-Z\-]*))?))?" +
            @"(?:\+(?<build>[0-9a-zA-Z\-]+(?:\.[0-9a-zA-Z\-]+)*))?$";
        private static readonly Regex SemverRegex = new Regex(SemverRegexPattern, RegexOptions.Compiled | RegexOptions.Singleline);

        public static Version Parse(string input)
        {
            var match = SemverRegex.Match(input);
            if (!match.Success) throw new ArgumentException($"{input} is not a valid Semantic Version!");

            return new Version(
                match.ParseUint("major"), 
                match.ParseUint("minor"), 
                match.ParseUint("patch"), 
                match.ParseIdentList("prerelease"),
                match.ParseIdentList("build"),
                input);
        }
        
    }

    public static class MatchExtensions
    {
        public static uint ParseUint(this Match match, string groupname)
        {
            return uint.Parse(match.Groups[groupname].Value);
        }

        public static Ident[] ParseIdentList(this Match match, string groupname)
        {
            var prereleaseGroup = match.Groups[groupname];
            return !prereleaseGroup.Success ? new Ident[] { } : prereleaseGroup.Value.ParseIdentList();
        }

        public static Ident[] ParseIdentList(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new Ident[] { };
            var elements = input.Split('.');
            return elements.Select(ParseIdent).ToArray();
        }

        public static Ident ParseIdent(this string input)
        {
            if (uint.TryParse(input, out var n)) return new Numeric(n);
            return new AlphaNumeric(input);
        }
    }

    public static class StringExtensions
    {
        public static Version ParseSemVer(this string input)
        {
            return SemVerParser.Parse(input);
        }
    }
}
