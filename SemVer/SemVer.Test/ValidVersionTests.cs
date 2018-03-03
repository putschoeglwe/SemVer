using System;
using Xunit;

namespace SemVer.Test
{
    public class ValidVersionTests

    {
        [Fact]
        public void GIVEN_a_three_part_version_WHEN_parsing_THEN_the_correct_version_is_created()
        {
               Assert.Equal(SemVerFactory.Create(1,0,0), "1.0.0".ParseSemVer());
        }

        [Fact]
        public void GIVEN_a_version_with_prerelease_part_WHEN_parsing_THEN_the_correct_version_is_created()
        {
            Assert.Equal(SemVerFactory.Create(1, 0, 0, new Ident[]{new AlphaNumeric("beta")}), "1.0.0-beta".ParseSemVer());
        }

        [Fact]
        public void GIVEN_a_version_with_build_part_WHEN_parsing_THEN_the_correct_version_is_created()
        {
            Assert.Equal(SemVerFactory.Create(1, 0, 0, build:new Ident[] { new Numeric(1) }), "1.0.0+1".ParseSemVer());
            Assert.Equal(SemVerFactory.Create(1, 0, 0, build: new Ident[] { new Numeric(1) }), "1.0.0+01".ParseSemVer());
            Assert.Equal(SemVerFactory.Create(1, 0, 0, build: new Ident[] { new AlphaNumeric("exp"), new AlphaNumeric("sha"), new Numeric(0x5114f85), }), "1.0.0+exp.sha.5114f85".ParseSemVer());
        }

        [Fact]
        public void GIVEN_a_version_with_prerelease_and_build_part_WHEN_parsing_THEN_the_correct_version_is_created()
        {
            Assert.Equal(SemVerFactory.Create(1, 0, 0, new Ident[] { new AlphaNumeric("alpha") },new Ident[] { new Numeric(1) }), "1.0.0-alpha+001".ParseSemVer());
            Assert.Equal(SemVerFactory.Create(1, 0, 0, new Ident[] { new AlphaNumeric("alpha") }, new Ident[] { new Numeric(20130313144700) }), "1.0.0-alpha+20130313144700".ParseSemVer());
        }                                                                                                                            
    }
}
