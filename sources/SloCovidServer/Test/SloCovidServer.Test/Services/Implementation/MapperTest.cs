using NUnit.Framework;
using SloCovidServer.Services.Implemented;

namespace SloCovidServer.Test.Services.Implementation
{
    public class MapperTest: BaseTest<Mapper>
    {
        [TestFixture]
        public class ParseLine: MapperTest
        {
            [Test]
            public void WhenEmptyLine_ReturnsSingleColumn()
            {
                var actual = Target.ParseLine("");

                Assert.That(actual, Is.EqualTo(new[] { "" }));
            }
            [Test]
            public void WhenSingleField_ReturnsOneColumn()
            {
                var actual = Target.ParseLine("test");

                Assert.That(actual, Is.EqualTo(new []{ "test" }));
            }
            [Test]
            public void WhenTwoFields_ReturnsTwoColumns()
            {
                var actual = Target.ParseLine("test,another");

                Assert.That(actual, Is.EqualTo(new[] { "test", "another" }));
            }
            [Test]
            public void WhenTwoFieldsAndSecondIsEmpty_ReturnsTwoColumns()
            {
                var actual = Target.ParseLine("test,");

                Assert.That(actual, Is.EqualTo(new[] { "test", "" }));
            }
            [Test]
            public void WhenSecondFieldsIsQuotedStringWithoutComma_ReturnsTwoColumns()
            {
                var actual = Target.ParseLine("test,\"yolo\"");

                Assert.That(actual, Is.EqualTo(new[] { "test", "yolo" }));
            }
            [Test]
            public void WhenSecondFieldsIsQuotedStringWithComma_ReturnsTwoColumns()
            {
                var actual = Target.ParseLine("test,\"yolo,bolo\"");

                Assert.That(actual, Is.EqualTo(new[] { "test", "yolo,bolo" }));
            }
            [Test]
            public void WhenSecondFieldsIsQuotedStringWithCommaFollowedWithEmpty_ReturnsThreeColumns()
            {
                var actual = Target.ParseLine("test,\"yolo,bolo\",");

                Assert.That(actual, Is.EqualTo(new[] { "test", "yolo,bolo", "" }));
            }
        }
    }
}
