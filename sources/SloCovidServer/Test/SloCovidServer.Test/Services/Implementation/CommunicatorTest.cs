using NUnit.Framework;
using SloCovidServer.Models;
using SloCovidServer.Services.Implemented;
using System;
using System.Collections.Immutable;

namespace SloCovidServer.Test.Services.Implementation
{
    public class CommunicatorTest: BaseTest<Communicator>
    {
        public class FilterData: CommunicatorTest
        {
            [TestFixture]
            public class UsingModelsWithDate: FilterData
            {
                [Test]
                public void WhenNoDataAndNoFilter_ReturnsEmpty()
                {
                    var data = ImmutableArray<ModelWithDate>.Empty;

                    var actual = Target.FilterData(data, new DataFilter(null, null));

                    Assert.That(actual.Length, Is.Zero);
                }
                [Test]
                public void WhenNoFilter_ReturnsAll()
                {
                    var data = ImmutableArray<ModelWithDate>.Empty.Add(new ModelWithDate(2000, 10, 20));

                    var actual = Target.FilterData(data, new DataFilter(null, null));

                    Assert.That(actual.Length, Is.EqualTo(1));
                }
                [Test]
                public void WhenFromFilterIsSet_ReturnsCorrectOne()
                {
                    var data = ImmutableArray<ModelWithDate>.Empty
                        .Add(new ModelWithDate(2000, 10, 19))
                        .Add(new ModelWithDate(2000, 10, 20));

                    var actual = Target.FilterData(data, new DataFilter(new DateTime(2000, 10, 20), null));

                    Assert.That(actual.Length, Is.EqualTo(1));
                    var item = actual[0];
                    Assert.That(item.Day, Is.EqualTo(20));
                }
                [Test]
                public void WhenToFilterIsSet_ReturnsCorrectOne()
                {
                    var data = ImmutableArray<ModelWithDate>.Empty
                        .Add(new ModelWithDate(2000, 10, 20))
                        .Add(new ModelWithDate(2000, 10, 21));

                    var actual = Target.FilterData(data, new DataFilter(null, new DateTime(2000, 10, 20)));

                    Assert.That(actual.Length, Is.EqualTo(1));
                    var item = actual[0];
                    Assert.That(item.Day, Is.EqualTo(20));
                }
                [Test]
                public void WhenToAndFromFilterIsSet_ReturnsCorrectTwoItems()
                {
                    var data = ImmutableArray<ModelWithDate>.Empty
                        .Add(new ModelWithDate(2000, 10, 18))
                        .Add(new ModelWithDate(2000, 10, 19))
                        .Add(new ModelWithDate(2000, 10, 20))
                        .Add(new ModelWithDate(2000, 10, 21));

                    var actual = Target.FilterData(data, new DataFilter(new DateTime(2000, 10, 19), new DateTime(2000, 10, 20)));

                    Assert.That(actual.Length, Is.EqualTo(2));
                    Assert.That(actual[0].Day, Is.EqualTo(19));
                    Assert.That(actual[1].Day, Is.EqualTo(20));
                }
            }
            [TestFixture]
            public class UsingModelsWithoutDate : FilterData
            {
                [Test]
                public void WhenNoDataAndNoFilter_ReturnsEmpty()
                {
                    var data = ImmutableArray<ModelWithoutDate>.Empty;

                    var actual = Target.FilterData(data, new DataFilter(null, null));

                    Assert.That(actual.Length, Is.Zero);
                }
                [TestFixture]
                public class WhenFilterIsSet_DoesNotAffectResults: UsingModelsWithoutDate
                {
                    [Test]
                    public void Test()
                    {
                        var data = ImmutableArray<ModelWithoutDate>.Empty.Add(new ModelWithoutDate());

                        var actual = Target.FilterData(data, new DataFilter(new DateTime(2000, 10, 20), new DateTime(2000, 9, 20)));

                        Assert.That(actual.Length, Is.EqualTo(1));
                    }
                }
            }
        }
    }

    public class ModelWithoutDate
    {

    }

    public class ModelWithDate : IModelDate
    {
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public ModelWithDate(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }
    }
}
