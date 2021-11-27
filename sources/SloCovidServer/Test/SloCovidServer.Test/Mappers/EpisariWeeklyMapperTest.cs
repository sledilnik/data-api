using NUnit.Framework;
using SloCovidServer.Mappers;

namespace SloCovidServer.Test.Mappers
{
    internal class EpisariWeeklyMapperTest: BaseTest<EpisariWeeklyMapper>
    {
        [TestFixture]
        public class ExtractAges: EpisariWeeklyMapperTest
        {
            [Test]
            public void WhenValidClosedRange_ParsesCorrectly()
            {
                var actual = Target.ExtractAges("12-24");

                Assert.That(actual, Is.EqualTo((12, 24)));
            }
            [Test]
            public void WhenValidOpenRange_ParsesCorrectly()
            {
                var actual = Target.ExtractAges("65+");

                Assert.That(actual, Is.EqualTo((65, (int?)null)));
            }
            [Test]
            public void WhenValueIsMean_ParsesAsOpen()
            {
                var actual = Target.ExtractAges("mean");

                Assert.That(actual, Is.EqualTo(((int?)null, (int?)null)));
            }
        }
    }
}
