// ******************************************************************************************************************************
// ****
// ****      Copyright (c) 2016-2021 Rafael 'Monoman' Teixeira
// ****
// ******************************************************************************************************************************

using System;
using System.Linq;

using Commons.Collections;

using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class UnitTestSquareList
    {
        [Test]
        public void TestConstructorWithSource() {
            var numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var sl = new SquareList<int>(16, numbers);
            Assert.That(sl.SequenceEqual(numbers));
            sl.Insert(13);
            sl.Insert(100);
            sl.Insert(39);
            sl.Insert(23);
            sl.Insert(93);
            sl.Insert(12);
            sl.Insert(14);
            sl.Insert(15);
            sl.Insert(16);
            sl.Insert(17);
            sl.Insert(18);
            sl.Insert(19);
            sl.Insert(20);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(100));
                Assert.That(sl.Size, Is.EqualTo(24));
            });
            sl.Delete(100);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(93));
                Assert.That(sl.Size, Is.EqualTo(23));
            });
        }

        [Test]
        public void TestConstructorWithSourceWithBadCapacity() {
            Exception exception = null;
            var numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            try {
                var sl = new SquareList<int>(9, numbers);
            } catch (Exception e) {
                exception = e;
            }
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.GetType(), Is.EqualTo(typeof(ArgumentException)));
        }

        [Test]
        public void TestConstructorWithSourceWithBadSequence() {
            Exception exception = null;
            var numbers = new int[] { 1, 2, 3, 6, 4, 9, 7, 8, 5, 10, 11 };
            try {
                var sl = new SquareList<int>(16, numbers);
            } catch (Exception e) {
                exception = e;
            }
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.GetType(), Is.EqualTo(typeof(ArgumentException)));
        }

        [Test]
        public void TestContains() {
            var sl = new SquareList<int>();
            sl.Insert(13);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(13));
                Assert.That(sl.Max, Is.EqualTo(13));
                Assert.That(sl.Contains(39), Is.EqualTo(false));
                Assert.That(sl.Contains(13), Is.EqualTo(true));
                Assert.That(sl.Contains(0), Is.EqualTo(false));
            });
        }

        [Test]
        public void TestDeleteOnEmpty() {
            var sl = new SquareList<int>();
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(0));
                Assert.That(sl.Max, Is.EqualTo(0));
                Assert.That(sl.Size, Is.EqualTo(0));
            });
            sl.Delete(13);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(0));
                Assert.That(sl.Max, Is.EqualTo(0));
                Assert.That(sl.Size, Is.EqualTo(0));
            });
        }

        [Test]
        public void TestDeleteOnNotFound() {
            var sl = new SquareList<int>();
            sl.Insert(1);
            sl.Insert(23);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(23));
                Assert.That(sl.Size, Is.EqualTo(2));
            });
            sl.Delete(13);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(23));
                Assert.That(sl.Size, Is.EqualTo(2));
            });
        }

        [Test]
        public void TestEmpty() {
            var sl = new SquareList<int>();
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(0));
                Assert.That(sl.Max, Is.EqualTo(0));
            });
            var slref = new SquareList<SimpleComparable>();
            Assert.Multiple(() => {
                Assert.That(slref.Min, Is.EqualTo(null));
                Assert.That(slref.Max, Is.EqualTo(null));
            });
        }

        private static readonly int[] _listWith_13_39_93 = [13, 39, 93];

        [Test]
        public void TestEnumeration() {
            var sl = new SquareList<int>();
            sl.Insert(13);
            sl.Insert(39);
            sl.Insert(93);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(13));
                Assert.That(sl.Max, Is.EqualTo(93));
                Assert.That(sl.Count(), Is.EqualTo(3));
                Assert.That(sl.SequenceEqual(_listWith_13_39_93));
            });
            var slref = new SquareList<SimpleComparable>();
            var c1 = new SimpleComparable(13);
            var c2 = new SimpleComparable(39);
            var c3 = new SimpleComparable(93);
            SimpleComparable[] expectedSequence = [c3, c2, c1];
            slref.Insert(c1);
            slref.Insert(c2);
            slref.Insert(c3);
            Assert.Multiple(() => {
                Assert.That(slref.Min, Is.SameAs(c3));
                Assert.That(slref.Max, Is.SameAs(c1));
                Assert.That(slref.Count(), Is.EqualTo(3));
                Assert.That(slref.SequenceEqual(expectedSequence));
            });
        }

        [Test]
        public void TestLarge() {
            var sl = new SquareList<int>();
            sl.Insert(13);
            sl.Insert(100);
            sl.Insert(39);
            sl.Insert(23);
            sl.Insert(93);
            sl.Insert(1);
            sl.Insert(2);
            sl.Insert(3);
            sl.Insert(4);
            sl.Insert(5);
            sl.Insert(6);
            sl.Insert(7);
            sl.Insert(8);
            sl.Insert(9);
            sl.Insert(10);
            sl.Insert(11);
            sl.Insert(12);
            sl.Insert(14);
            sl.Insert(15);
            sl.Insert(16);
            sl.Insert(17);
            sl.Insert(18);
            sl.Insert(19);
            sl.Insert(20);
            sl.Insert(21);
            sl.Insert(22);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(100));
                Assert.That(sl.Size, Is.EqualTo(26));
            });
        }

        [Test]
        public void TestLargeWithDeletes() {
            var sl = new SquareList<int>();
            sl.Insert(13);
            sl.Insert(100);
            sl.Insert(39);
            sl.Insert(23);
            sl.Insert(93);
            sl.Insert(1);
            sl.Insert(2);
            sl.Insert(3);
            sl.Insert(4);
            sl.Insert(5);
            sl.Insert(6);
            sl.Insert(7);
            sl.Insert(8);
            sl.Insert(9);
            sl.Insert(10);
            sl.Insert(11);
            sl.Insert(12);
            sl.Insert(14);
            sl.Insert(15);
            sl.Insert(16);
            sl.Insert(17);
            sl.Insert(18);
            sl.Insert(19);
            sl.Insert(20);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(100));
                Assert.That(sl.Size, Is.EqualTo(24));
            });
            sl.Delete(100);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(93));
                Assert.That(sl.Size, Is.EqualTo(23));
            });
        }

        [Test]
        public void TestMultipleInsertAndMultipleDelete() {
            var sl = new SquareList<int>();
            sl.Insert(1);
            sl.Insert(2);
            sl.Insert(3);
            sl.Insert(4);
            sl.Insert(5);
            sl.Insert(6);
            sl.Insert(7);
            sl.Insert(8);
            sl.Insert(9);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(9));
                Assert.That(sl.Size, Is.EqualTo(9));
            });
            sl.Delete(1);
            sl.Delete(2);
            sl.Delete(3);
            sl.Delete(4);
            sl.Delete(5);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(6));
                Assert.That(sl.Max, Is.EqualTo(9));
                Assert.That(sl.Size, Is.EqualTo(4));
            });
        }

        [Test]
        public void TestQuadrupleInsertAndDoubleDeleteFirstOnes() {
            var sl = new SquareList<int>();
            sl.Insert(1);
            sl.Insert(2);
            sl.Insert(3);
            sl.Insert(4);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(4));
                Assert.That(sl.Size, Is.EqualTo(4));
            });
            sl.Delete(1);
            sl.Delete(2);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(3));
                Assert.That(sl.Max, Is.EqualTo(4));
                Assert.That(sl.Size, Is.EqualTo(2));
            });
        }

        [Test]
        public void TestQuadrupleInsertAndDoubleDeleteLastOnes() {
            var sl = new SquareList<int>();
            sl.Insert(1);
            sl.Insert(2);
            sl.Insert(3);
            sl.Insert(4);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(4));
                Assert.That(sl.Size, Is.EqualTo(4));
            });
            sl.Delete(3);
            sl.Delete(4);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(2));
                Assert.That(sl.Size, Is.EqualTo(2));
            });
        }

        [Test]
        public void TestShrink() {
            var sl = new SquareList<int>(9);
            sl.Insert(13);
            sl.Insert(100);
            sl.Insert(39);
            sl.Insert(23);
            sl.Insert(93);
            sl.Insert(1);
            sl.Insert(2);
            sl.Insert(3);
            sl.Insert(4);
            sl.Insert(5);
            sl.Insert(6);
            sl.Insert(7);
            sl.Insert(8);
            sl.Insert(9);
            sl.Insert(10);
            sl.Insert(11);
            sl.Insert(12);
            sl.Insert(14);
            sl.Insert(15);
            sl.Insert(16);
            sl.Insert(17);
            sl.Insert(18);
            sl.Insert(19);
            sl.Insert(20);
            sl.Insert(21);
            sl.Insert(22);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(100));
                Assert.That(sl.Size, Is.EqualTo(26));
                Assert.That(sl.Capacity, Is.EqualTo(30));
                Assert.That(sl.Ratio, Is.EqualTo(86));
            });
            sl.Delete(2);
            sl.Delete(3);
            sl.Delete(4);
            sl.Delete(5);
            sl.Delete(6);
            sl.Delete(7);
            sl.Delete(8);
            sl.Delete(9);
            sl.Delete(10);
            sl.Delete(11);
            sl.Delete(12);
            sl.Delete(14);
            sl.Delete(15);
            sl.Delete(16);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(100));
                Assert.That(sl.Size, Is.EqualTo(12));
                Assert.That(sl.Capacity, Is.EqualTo(30));
                Assert.That(sl.Ratio, Is.EqualTo(40));
            });
            sl.ShrinkWithSlackOf(1);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(100));
                Assert.That(sl.Size, Is.EqualTo(12));
                Assert.That(sl.Capacity, Is.EqualTo(30));
                Assert.That(sl.Ratio, Is.EqualTo(40));
            });
            sl.ShrinkWithSlackOf(0);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(100));
                Assert.That(sl.Size, Is.EqualTo(12));
                Assert.That(sl.Capacity, Is.EqualTo(20));
                Assert.That(sl.Ratio, Is.EqualTo(60));
            });
        }

        [Test]
        public void TestSingle() {
            var sl = new SquareList<int>();
            sl.Insert(13);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(13));
                Assert.That(sl.Max, Is.EqualTo(13));
            });
            var slref = new SquareList<SimpleComparable>();
            var c = new SimpleComparable(13);
            slref.Insert(c);
            Assert.Multiple(() => {
                Assert.That(slref.Min, Is.SameAs(c));
                Assert.That(slref.Max, Is.SameAs(c));
            });
        }

        [Test]
        public void TestSingleInsertAndDelete() {
            var sl = new SquareList<int>();
            sl.Insert(13);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(13));
                Assert.That(sl.Max, Is.EqualTo(13));
                Assert.That(sl.Size, Is.EqualTo(1));
            });
            sl.Delete(13);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(0));
                Assert.That(sl.Max, Is.EqualTo(0));
                Assert.That(sl.Size, Is.EqualTo(0));
            });
        }

        [Test]
        public void TestSmall() {
            var sl = new SquareList<int>();
            sl.Insert(13);
            sl.Insert(39);
            sl.Insert(93);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(13));
                Assert.That(sl.Max, Is.EqualTo(93));
            });
            var slref = new SquareList<SimpleComparable>();
            var c1 = new SimpleComparable(13);
            var c2 = new SimpleComparable(39);
            var c3 = new SimpleComparable(93);
            slref.Insert(c1);
            slref.Insert(c2);
            slref.Insert(c3);
            Assert.Multiple(() => {
                Assert.That(slref.Min, Is.SameAs(c3));
                Assert.That(slref.Max, Is.SameAs(c1));
            });
        }

        [Test]
        public void TestSmallWithRepetitions() {
            var sl = new SquareList<int>();
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(15);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(13));
                Assert.That(sl.Max, Is.EqualTo(15));
                Assert.That(sl.Size, Is.EqualTo(10));
            });
            sl.Delete(13); // only the first;
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(13));
                Assert.That(sl.Max, Is.EqualTo(15));
                Assert.That(sl.Size, Is.EqualTo(9));
            });
            sl.Delete(13, true); // all remaining;
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(15));
                Assert.That(sl.Max, Is.EqualTo(15));
                Assert.That(sl.Size, Is.EqualTo(1));
            });
            sl.Delete(15, true); // all;
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(0));
                Assert.That(sl.Max, Is.EqualTo(0));
                Assert.That(sl.Size, Is.EqualTo(0));
            });
        }

        [Test]
        public void TestUnremove() {
            var sl = new SquareList<int>();
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(13);
            sl.Insert(15);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(13));
                Assert.That(sl.Max, Is.EqualTo(15));
                Assert.That(sl.Size, Is.EqualTo(10));
            });
            sl.Delete(13, true); // all of them;
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(15));
                Assert.That(sl.Max, Is.EqualTo(15));
                Assert.That(sl.Size, Is.EqualTo(1));
            });
            sl.Insert(1);
            sl.Insert(2);
            sl.Insert(3);
            sl.Insert(4);
            sl.Insert(5);
            sl.Insert(6);
            sl.Insert(7);
            sl.Insert(8);
            sl.Insert(9);
            sl.Insert(10);
            sl.Insert(11);
            sl.Insert(12);
            sl.Insert(13);
            sl.Insert(14);
            Assert.Multiple(() => {
                Assert.That(sl.Min, Is.EqualTo(1));
                Assert.That(sl.Max, Is.EqualTo(15));
                Assert.That(sl.Size, Is.EqualTo(15));
            });
        }

        private class SimpleComparable(int val) : IComparable
        {
            public int CompareTo(object obj) => ((SimpleComparable)obj)._value - _value;

            public override string ToString() => $"|{_value}|";

            private readonly int _value = val;
        }
    }
}