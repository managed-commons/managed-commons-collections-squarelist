using System;
using System.Linq;
using Commons.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitTestSquareList
    {
        [TestMethod]
        public void TestConstructorWithSource()
        {
            var numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var sl = new SquareList<int>(16, numbers);
            Assert.IsTrue(sl.SequenceEqual(numbers));
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
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(100, sl.Max);
            Assert.AreEqual(24, sl.Size);
            sl.Delete(100);
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(93, sl.Max);
            Assert.AreEqual(23, sl.Size);
        }

        [TestMethod]
        public void TestConstructorWithSourceWithBadCapacity()
        {
            Exception exception = null;
            var numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            try {
                var sl = new SquareList<int>(9, numbers);
            } catch (Exception e) {
                exception = e;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(typeof(ArgumentException), exception.GetType());
        }

        [TestMethod]
        public void TestConstructorWithSourceWithBadSequence()
        {
            Exception exception = null;
            var numbers = new int[] { 1, 2, 3, 6, 4, 9, 7, 8, 5, 10, 11 };
            try {
                var sl = new SquareList<int>(16, numbers);
            } catch (Exception e) {
                exception = e;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(typeof(ArgumentException), exception.GetType());
        }

        [TestMethod]
        public void TestContains()
        {
            var sl = new SquareList<int>();
            sl.Insert(13);
            Assert.AreEqual(13, sl.Min);
            Assert.AreEqual(13, sl.Max);
            Assert.AreEqual(false, sl.Contains(39));
            Assert.AreEqual(true, sl.Contains(13));
            Assert.AreEqual(false, sl.Contains(0));
        }

        [TestMethod]
        public void TestDeleteOnEmpty()
        {
            var sl = new SquareList<int>();
            Assert.AreEqual(0, sl.Min);
            Assert.AreEqual(0, sl.Max);
            Assert.AreEqual(0, sl.Size);
            sl.Delete(13);
            Assert.AreEqual(0, sl.Min);
            Assert.AreEqual(0, sl.Max);
            Assert.AreEqual(0, sl.Size);
        }

        [TestMethod]
        public void TestDeleteOnNotFound()
        {
            var sl = new SquareList<int>();
            sl.Insert(1);
            sl.Insert(23);
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(23, sl.Max);
            Assert.AreEqual(2, sl.Size);
            sl.Delete(13);
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(23, sl.Max);
            Assert.AreEqual(2, sl.Size);
        }

        [TestMethod]
        public void TestEmpty()
        {
            var sl = new SquareList<int>();
            Assert.AreEqual(0, sl.Min);
            Assert.AreEqual(0, sl.Max);
            var slref = new SquareList<SimpleComparable>();
            Assert.AreEqual(null, slref.Min);
            Assert.AreEqual(null, slref.Max);
        }

        [TestMethod]
        public void TestEnumeration()
        {
            var sl = new SquareList<int>();
            sl.Insert(13);
            sl.Insert(39);
            sl.Insert(93);
            Assert.AreEqual(13, sl.Min);
            Assert.AreEqual(93, sl.Max);
            Assert.AreEqual(3, sl.Count());
            Assert.IsTrue(sl.SequenceEqual(new int[] { 13, 39, 93 }));
            var slref = new SquareList<SimpleComparable>();
            var c1 = new SimpleComparable(13);
            var c2 = new SimpleComparable(39);
            var c3 = new SimpleComparable(93);
            slref.Insert(c1);
            slref.Insert(c2);
            slref.Insert(c3);
            Assert.AreSame(c3, slref.Min);
            Assert.AreSame(c1, slref.Max);
            Assert.AreEqual(3, slref.Count());
            Assert.IsTrue(slref.SequenceEqual(new SimpleComparable[] { c3, c2, c1 }));
        }

        [TestMethod]
        public void TestLarge()
        {
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
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(100, sl.Max);
            Assert.AreEqual(26, sl.Size);
        }

        [TestMethod]
        public void TestLargeWithDeletes()
        {
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
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(100, sl.Max);
            Assert.AreEqual(24, sl.Size);
            sl.Delete(100);
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(93, sl.Max);
            Assert.AreEqual(23, sl.Size);
        }

        [TestMethod]
        public void TestMultipleInsertAndMultipleDelete()
        {
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
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(9, sl.Max);
            Assert.AreEqual(9, sl.Size);
            sl.Delete(1);
            sl.Delete(2);
            sl.Delete(3);
            sl.Delete(4);
            sl.Delete(5);
            Assert.AreEqual(6, sl.Min);
            Assert.AreEqual(9, sl.Max);
            Assert.AreEqual(4, sl.Size);
        }

        [TestMethod]
        public void TestQuadrupleInsertAndDoubleDeleteFirstOnes()
        {
            var sl = new SquareList<int>();
            sl.Insert(1);
            sl.Insert(2);
            sl.Insert(3);
            sl.Insert(4);
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(4, sl.Max);
            Assert.AreEqual(4, sl.Size);
            sl.Delete(1);
            sl.Delete(2);
            Assert.AreEqual(3, sl.Min);
            Assert.AreEqual(4, sl.Max);
            Assert.AreEqual(2, sl.Size);
        }

        [TestMethod]
        public void TestQuadrupleInsertAndDoubleDeleteLastOnes()
        {
            var sl = new SquareList<int>();
            sl.Insert(1);
            sl.Insert(2);
            sl.Insert(3);
            sl.Insert(4);
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(4, sl.Max);
            Assert.AreEqual(4, sl.Size);
            sl.Delete(3);
            sl.Delete(4);
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(2, sl.Max);
            Assert.AreEqual(2, sl.Size);
        }

        [TestMethod]
        public void TestShrink()
        {
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
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(100, sl.Max);
            Assert.AreEqual(26, sl.Size);
            Assert.AreEqual(30, sl.Capacity);
            Assert.AreEqual(86, sl.Ratio);
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
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(100, sl.Max);
            Assert.AreEqual(12, sl.Size);
            Assert.AreEqual(30, sl.Capacity);
            Assert.AreEqual(40, sl.Ratio);
            sl.ShrinkWithSlackOf(1);
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(100, sl.Max);
            Assert.AreEqual(12, sl.Size);
            Assert.AreEqual(30, sl.Capacity);
            Assert.AreEqual(40, sl.Ratio);
            sl.ShrinkWithSlackOf(0);
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(100, sl.Max);
            Assert.AreEqual(12, sl.Size);
            Assert.AreEqual(20, sl.Capacity);
            Assert.AreEqual(60, sl.Ratio);
        }

        [TestMethod]
        public void TestSingle()
        {
            var sl = new SquareList<int>();
            sl.Insert(13);
            Assert.AreEqual(13, sl.Min);
            Assert.AreEqual(13, sl.Max);
            var slref = new SquareList<SimpleComparable>();
            var c = new SimpleComparable(13);
            slref.Insert(c);
            Assert.AreSame(c, slref.Min);
            Assert.AreSame(c, slref.Max);
        }

        [TestMethod]
        public void TestSingleInsertAndDelete()
        {
            var sl = new SquareList<int>();
            sl.Insert(13);
            Assert.AreEqual(13, sl.Min);
            Assert.AreEqual(13, sl.Max);
            Assert.AreEqual(1, sl.Size);
            sl.Delete(13);
            Assert.AreEqual(0, sl.Min);
            Assert.AreEqual(0, sl.Max);
            Assert.AreEqual(0, sl.Size);
        }

        [TestMethod]
        public void TestSmall()
        {
            var sl = new SquareList<int>();
            sl.Insert(13);
            sl.Insert(39);
            sl.Insert(93);
            Assert.AreEqual(13, sl.Min);
            Assert.AreEqual(93, sl.Max);
            var slref = new SquareList<SimpleComparable>();
            var c1 = new SimpleComparable(13);
            var c2 = new SimpleComparable(39);
            var c3 = new SimpleComparable(93);
            slref.Insert(c1);
            slref.Insert(c2);
            slref.Insert(c3);
            Assert.AreSame(c3, slref.Min);
            Assert.AreSame(c1, slref.Max);
        }

        [TestMethod]
        public void TestSmallWithRepetitions()
        {
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
            Assert.AreEqual(13, sl.Min);
            Assert.AreEqual(15, sl.Max);
            Assert.AreEqual(10, sl.Size);
            sl.Delete(13); // only the first;
            Assert.AreEqual(13, sl.Min);
            Assert.AreEqual(15, sl.Max);
            Assert.AreEqual(9, sl.Size);
            sl.Delete(13, true); // all remaining;
            Assert.AreEqual(15, sl.Min);
            Assert.AreEqual(15, sl.Max);
            Assert.AreEqual(1, sl.Size);
            sl.Delete(15, true); // all;
            Assert.AreEqual(0, sl.Min);
            Assert.AreEqual(0, sl.Max);
            Assert.AreEqual(0, sl.Size);
        }

        [TestMethod]
        public void TestUnremove()
        {
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
            Assert.AreEqual(13, sl.Min);
            Assert.AreEqual(15, sl.Max);
            Assert.AreEqual(10, sl.Size);
            sl.Delete(13, true); // all of them;
            Assert.AreEqual(15, sl.Min);
            Assert.AreEqual(15, sl.Max);
            Assert.AreEqual(1, sl.Size);
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
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(15, sl.Max);
            Assert.AreEqual(15, sl.Size);
        }

        private class SimpleComparable : IComparable
        {
            public SimpleComparable(int val)
            {
                value = val;
            }

            public int CompareTo(object obj)
            {
                return ((SimpleComparable)obj).value - value;
            }

            public override string ToString()
            {
                return $"|{value}|";
            }

            private readonly int value;
        }
    }
}
