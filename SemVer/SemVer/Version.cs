using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SemVer
{
    public sealed class Version : IEquatable<Version>, IComparable<Version>
    {
        public uint Major { get; }
        public uint Minor { get; }
        public uint Patch { get; }
        public Ident[] Prerelease { get; }
        public Ident[] Build { get; }
        
        public string Original { get; }

        internal Version(uint major, uint minor, uint patch, Ident[] prerelease, Ident[] build, string original)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Prerelease = prerelease;
            Build = build;
            Original = original;
        }


        public bool IsPrerelease => this.Prerelease.Any();

        public int CompareTo(Version other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var majorComparison = Major.CompareTo(other.Major);
            if (majorComparison != 0) return majorComparison;
            var minorComparison = Minor.CompareTo(other.Minor);
            if (minorComparison != 0) return minorComparison;
            var patchComparison = Patch.CompareTo(other.Patch);
            if (patchComparison != 0) return patchComparison;
            if (!IsPrerelease && !other.IsPrerelease) return 0;
            if (!IsPrerelease) return 1;
            if (!other.IsPrerelease) return -1;
            return CompareNonEmptyIdents(Prerelease, other.Prerelease);
        }

        public static bool operator <(Version left, Version right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Version left, Version right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Version left, Version right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Version left, Version right)
        {
            return left.CompareTo(right) >= 0;
        }

        private static int CompareNonEmptyIdents(Ident[] first, Ident[] second)
        {
            var min = first.Length < second.Length ? first.Length : second.Length;
            for (int i = 0; i < min; i++)
            {
                var r = CompareIdent(first[i], second[i]);
                if (r != 0) return r;
            }
            return first.Length == second.Length ? 0 : first.Length < second.Length ? -1 : 1;
        }

        private static int CompareIdent(Ident ident, Ident other)
        {
            if (ident is Numeric n)
            {
                if (other is Numeric no) return n.Number.CompareTo(no.Number);
                return -1;
            }

            if (other is Numeric ) return -1;
            if (ident is AlphaNumeric an1 && other is AlphaNumeric an2)
            {
                var s1 = an1.Value;
                var s2 = an2.Value;
                return string.Compare(s1, s2, StringComparison.Ordinal);
            }

            throw new NotImplementedException("Ident must either be Numeric or Alphanumeric, but was something else!");
        }

        public bool Equals(Version other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CompareTo(other) == 0; 
        }

        public static bool operator ==(Version left, Version right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Version left, Version right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Version version && Equals(version);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Major;
                hashCode = (hashCode * 397) ^ (int) Minor;
                hashCode = (hashCode * 397) ^ (int) Patch;
                hashCode = (hashCode * 397) ^ (Prerelease != null ? Prerelease.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Build != null ? Build.GetHashCode() : 0);
                return hashCode;
            }
        }

    }

    public class Numeric : Ident
    {
        public ulong Number { get; }


        public Numeric(ulong n)
        {
            Number = n;
        }
    }

    public class AlphaNumeric : Ident
    {
        public AlphaNumeric(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
