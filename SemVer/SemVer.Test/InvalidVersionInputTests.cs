using System;
using Xunit;

namespace SemVer.Test
{
    public class InvalidVersionInputTests
    {
        [Theory]
        [InlineData("1")]
        [InlineData("1.")]
        [InlineData("1.2")]
        [InlineData("1.2.")]
        [InlineData("1.2.+1")]
        [InlineData("1.2.-alpha")]
        [InlineData("1.2.-alpha+1")]
        public void GIVEN_a_version_with_not_exactly_tree_numbers_WHEN_parsing_THEN_an_exception_is_raised(string input)
        {
            Exception ex = Assert.Throws<ArgumentException>(() => input.ParseSemVer());
            Assert.Equal($"{input} is not a valid Semantic Version!", ex.Message);
        }

        [Theory]
        [InlineData("01.0.0")]
        [InlineData("1.02.0")]
        [InlineData("1.0.03")]
        [InlineData("1.0.0-alpha.01")]
        public void GIVEN_a_version_with_leading_zeros_WHEN_parsing_THEN_an_exception_is_raised(string input)
        {
            Exception ex = Assert.Throws<ArgumentException>(() => input.ParseSemVer());
            Assert.Equal($"{input} is not a valid Semantic Version!", ex.Message);
        }

        [Theory]
        [InlineData("1.0.0-")]
        [InlineData("1.2.0+")]
        [InlineData("1.0.0-+")]
        public void GIVEN_a_version_with_empty_prerelease_or_build_parts_WHEN_parsing_THEN_an_exception_is_raised(string input)
        {
            Exception ex = Assert.Throws<ArgumentException>(() => input.ParseSemVer());
            Assert.Equal($"{input} is not a valid Semantic Version!", ex.Message);
        }

        [Fact]
        public void GIVEN_an_input_with_multiple_plus_chars_WHEN_parsing_THEN_an_exception_is_raised()
        {
            var input = "1.0.0+2+we";
            Exception ex = Assert.Throws<ArgumentException>(() => input.ParseSemVer());
            Assert.Equal($"{input} is not a valid Semantic Version!", ex.Message);
        }
    }
}