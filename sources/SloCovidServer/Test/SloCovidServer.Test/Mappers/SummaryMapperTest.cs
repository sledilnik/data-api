using NUnit.Framework;
using SloCovidServer.Mappers;
using SloCovidServer.Models;
using System;
using System.Collections.Immutable;

namespace SloCovidServer.Test.Mappers
{
    public class SummaryMapperTest
    {
        [TestFixture]
        public class GetLastItemOnDate : SummaryMapperTest
        {
            [Test]
            public void WhenNoItems_ReturnsNull()
            {
                var items = ImmutableArray<TestSummaryItem>.Empty;

                var actual = SummaryMapper.GetLastItemOnDate(new DateTime(2020, 12, 18), items, i => true);

                Assert.That(actual, Is.Null);
            }
            [Test]
            public void WhenOnlyMoreRecentDates_ReturnsNull()
            {
                var items = ImmutableArray<TestSummaryItem>.Empty
                    .Add(new TestSummaryItem(2020, 12, 30))
                    .Add(new TestSummaryItem(2020, 12, 31));

                var actual = SummaryMapper.GetLastItemOnDate(new DateTime(2020, 12, 18), items, i => true);

                Assert.That(actual, Is.Null);
            }
            [Test]
            public void WhenItemWithMatchingDateExistsAndIsLast_ReturnsItemWithSameDateAndIndexOne()
            {
                var items = ImmutableArray<TestSummaryItem>.Empty
                    .Add(new TestSummaryItem(2020, 12, 17))
                    .Add(new TestSummaryItem(2020, 12, 18));

                var actual = SummaryMapper.GetLastItemOnDate(new DateTime(2020, 12, 18), items, i => true);

                Assert.That(actual.Value.Item, Is.SameAs(items[1]));
                Assert.That(actual.Value.Index, Is.EqualTo(1));
            }
            [Test]
            public void WhenItemWithMatchingDateExistsAndIsNotLast_ReturnsItemWithSameDateAndIndexOne()
            {
                var items = ImmutableArray<TestSummaryItem>.Empty
                    .Add(new TestSummaryItem(2020, 12, 17))
                    .Add(new TestSummaryItem(2020, 12, 18))
                    .Add(new TestSummaryItem(2020, 12, 19));

                var actual = SummaryMapper.GetLastItemOnDate(new DateTime(2020, 12, 18), items, i => true);

                Assert.That(actual.Value.Item, Is.SameAs(items[1]));
                Assert.That(actual.Value.Index, Is.EqualTo(1));
            }
        }
        [TestFixture]
        public class GetLastAndPreviousItem : SummaryMapperTest
        {
            [Test]
            public void WhenNoItems_ReturnsNull()
            {
                var items = ImmutableArray<TestSummaryItem>.Empty;

                var actual = SummaryMapper.GetLastAndPreviousItem(new DateTime(2020, 12, 18), items, i => true);

                Assert.That(actual, Is.Null);
            }
            [Test]
            public void WhenItemWithMatchingDateExistsAndNoPrevious_ReturnsPreviousAsNull()
            {
                var items = ImmutableArray<TestSummaryItem>.Empty
                    .Add(new TestSummaryItem(2020, 12, 18));

                var actual = SummaryMapper.GetLastAndPreviousItem(new DateTime(2020, 12, 18), items, i => true);

                Assert.That(actual.Value.Previous, Is.Null);
            }
            [Test]
            public void WhenItemWithMatchingDateExistsAndAlsoPrevious_ReturnsPreviousCorrectly()
            {
                var items = ImmutableArray<TestSummaryItem>.Empty
                    .Add(new TestSummaryItem(2020, 12, 17))
                    .Add(new TestSummaryItem(2020, 12, 18));

                var actual = SummaryMapper.GetLastAndPreviousItem(new DateTime(2020, 12, 18), items, i => true);

                Assert.That(actual.Value.Previous, Is.SameAs(items[0]));
            }
            [Test]
            public void WhenNoItemsAndNoDate_ReturnsNull()
            {
                var items = ImmutableArray<TestSummaryItem>.Empty;

                var actual = SummaryMapper.GetLastAndPreviousItem(null, items, i => true);

                Assert.That(actual, Is.Null);
            }
            [Test]
            public void WhenOnlySingleItemExistsAndNoDate_ReturnsPreviousAsNull()
            {
                var items = ImmutableArray<TestSummaryItem>.Empty
                    .Add(new TestSummaryItem(2020, 12, 18));

                var actual = SummaryMapper.GetLastAndPreviousItem(null, items, i => true);

                Assert.That(actual.Value.Previous, Is.Null);
            }
            [Test]
            public void WhenOnlySingleItemExistsAndNoDate_ReturnsLastAsLastItem()
            {
                var items = ImmutableArray<TestSummaryItem>.Empty
                    .Add(new TestSummaryItem(2020, 12, 18));

                var actual = SummaryMapper.GetLastAndPreviousItem(null, items, i => true);

                Assert.That(actual.Value.Last, Is.SameAs(items[0]));
            }
            [Test]
            public void WhenAtLeastTwoItemsExistAndNoDate_ReturnsPreviousCorrectly()
            {
                var items = ImmutableArray<TestSummaryItem>.Empty
                    .Add(new TestSummaryItem(2020, 12, 17))
                    .Add(new TestSummaryItem(2020, 12, 18));

                var actual = SummaryMapper.GetLastAndPreviousItem(null, items, i => true);

                Assert.That(actual.Value.Previous, Is.SameAs(items[0]));
            }
        }
        [TestFixture]
        public class CalculateDifference: SummaryMapperTest
        {
            [TestCase(null, null, ExpectedResult = null)]
            [TestCase(4, null, ExpectedResult = null)]
            [TestCase(null, 2, ExpectedResult = null)]
            [TestCase(4, 2, ExpectedResult = 2f)]
            [TestCase(5, 2, ExpectedResult = 2.5f)]
            public float? GiveValus_ReturnsExpectedResult(int? last, int? previous)
            {
                return SummaryMapper.CalculateDifference(last, previous);
            }
        }
        [TestFixture]
        public class GetCasesAvg7Days: SummaryMapperTest
        {
            StatsDaily CreateSimplifiedStats(int? confirmedToday) => new StatsDaily(default, default, default, default, default, default, default, default, default,
                default, default, default, 
                new Cases(confirmedToday, default, default, default,default, default, default, default), 
                default, default, default, default);
            [Test]
            public void WhenNoValues_ReturnsNull()
            {
                var stats = ImmutableArray<StatsDaily>.Empty;

                var actual = SummaryMapper.GetCasesAvg7Days(default, stats);

                Assert.That(actual, Is.Null);
            }
            [Test]
            public void WhenAll7ValuesExist_ReturnsAverage()
            {
                var stats = ImmutableArray<StatsDaily>.Empty
                    .Add(CreateSimplifiedStats(0))
                    .Add(CreateSimplifiedStats(1))
                    .Add(CreateSimplifiedStats(2))
                    .Add(CreateSimplifiedStats(3))
                    .Add(CreateSimplifiedStats(4))
                    .Add(CreateSimplifiedStats(5))
                    .Add(CreateSimplifiedStats(6));

                var actual = SummaryMapper.GetCasesAvg7Days(toDate: default, stats);

                Assert.That(actual.Value, Is.EqualTo(3));
            }
            [Test]
            public void WhenAllLessThan5ValuesExist_ReturnsAverage()
            {
                var stats = ImmutableArray<StatsDaily>.Empty
                    .Add(CreateSimplifiedStats(0))
                    .Add(CreateSimplifiedStats(1))
                    .Add(CreateSimplifiedStats(2))
                    .Add(CreateSimplifiedStats(3))
                    .Add(CreateSimplifiedStats(4));

                var actual = SummaryMapper.GetCasesAvg7Days(toDate: default, stats);

                Assert.That(actual.Value, Is.EqualTo(2));
            }
            [Test]
            public void WhenAllLessThan5ValuesExist_DiffIsNull()
            {
                var stats = ImmutableArray<StatsDaily>.Empty
                    .Add(CreateSimplifiedStats(0))
                    .Add(CreateSimplifiedStats(1))
                    .Add(CreateSimplifiedStats(2))
                    .Add(CreateSimplifiedStats(3))
                    .Add(CreateSimplifiedStats(4));

                var actual = SummaryMapper.GetCasesAvg7Days(toDate: default, stats);

                Assert.That(actual.DiffPercentage, Is.Null);
            }
            [Test]
            public void WhenAll7ValuesExist_DiffIsNull()
            {
                var stats = ImmutableArray<StatsDaily>.Empty
                    .Add(CreateSimplifiedStats(0))
                    .Add(CreateSimplifiedStats(1))
                    .Add(CreateSimplifiedStats(2))
                    .Add(CreateSimplifiedStats(3))
                    .Add(CreateSimplifiedStats(4))
                    .Add(CreateSimplifiedStats(5))
                    .Add(CreateSimplifiedStats(6));

                var actual = SummaryMapper.GetCasesAvg7Days(toDate: default, stats);

                Assert.That(actual.DiffPercentage, Is.Null);
            }
            [Test]
            public void WhenMoreThan7ValuesExist_DiffIsCalculatedProperly()
            {
                var stats = ImmutableArray<StatsDaily>.Empty
                    .Add(CreateSimplifiedStats(0))
                    .Add(CreateSimplifiedStats(1))
                    .Add(CreateSimplifiedStats(2))
                    .Add(CreateSimplifiedStats(3))
                    .Add(CreateSimplifiedStats(4))
                    .Add(CreateSimplifiedStats(5))
                    .Add(CreateSimplifiedStats(6))
                    .Add(CreateSimplifiedStats(7));

                var actual = SummaryMapper.GetCasesAvg7Days(toDate: default, stats);

                Assert.That(actual.DiffPercentage, Is.EqualTo(4/3f));
            }
            [Test]
            public void WhenMoreThan7ValuesExist_AverageCalculatesProperValues()
            {
                var stats = ImmutableArray<StatsDaily>.Empty
                    .Add(CreateSimplifiedStats(8))
                    .Add(CreateSimplifiedStats(1))
                    .Add(CreateSimplifiedStats(2))
                    .Add(CreateSimplifiedStats(3))
                    .Add(CreateSimplifiedStats(4))
                    .Add(CreateSimplifiedStats(5))
                    .Add(CreateSimplifiedStats(6))
                    .Add(CreateSimplifiedStats(7));

                var actual = SummaryMapper.GetCasesAvg7Days(toDate: default, stats);

                Assert.That(actual.Value, Is.EqualTo(4));
            }
        }
    }

    public record TestSummaryItem(int Year, int Month, int Day) : IModelDate;
}
