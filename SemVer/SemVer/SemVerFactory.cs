using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SemVer
{
    public static class SemVerFactory
    {
        public static Version Create(uint major, uint minor, uint patch, Ident[] prerelease=null, Ident[] build = null)
        {
            var p = new Ident[] { };
            var b = new Ident[] { };
            if (prerelease != null) p = prerelease;
            if (build != null) b = build;
            return new Version(major, minor, patch, p, b, string.Empty);
        }

        public static Version AddPrerelease(this Version version, string alphaNumeric)
        {
            var p = version.Prerelease.Concat(new Ident[] {new AlphaNumeric(alphaNumeric)}).ToArray();
            return Create(version.Major, version.Minor, version.Patch, prerelease: p);
        }
    }
}
