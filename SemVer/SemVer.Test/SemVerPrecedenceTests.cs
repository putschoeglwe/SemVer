using Xunit;

namespace SemVer.Test
{
    public class SemVerPrecedenceTests
    {
        [Theory]
        [InlineData("1.0.0-alpha", "1.0.0", "When major, minor, and patch are equal, a pre-release version has lower precedence than a normal version.")]
        [InlineData("1.0.0-beta.9", "1.0.0-beta.12", "identifiers consisting of only digits are compared numerically")]
        [InlineData("1.0.0-beta", "1.0.0-rc", "identifiers with letters or hyphens are compared lexically in ASCII sort order")]
        [InlineData("1.0.0-alpha.1", "1.0.0-alpha.beta", "Numeric identifiers always have lower precedence than non-numeric identifiers.")]
        [InlineData("1.0.0-beta", "1.0.0-beta.2", "A larger set of pre-release fields has a higher precedence than a smaller set, if all of the preceding identifiers are equal.")]
        public void GIVEN_two_valid_versions_WHEN_comparing_THEN_the_precedence_is_calculated_correctly(string lower, string higher, string msg)
        {
            Assert.True(lower.ParseSemVer() < higher.ParseSemVer(), msg);
        }

        [Fact]
        public void Given_two_valid_version_with_build_parts_WHEN_comparing_THEN_the_build_part_is_ignored()
        {
            Assert.True("1.0.0+4".ParseSemVer() == "1.0.0+234".ParseSemVer());
        }
    }
}