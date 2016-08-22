using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commons.Collections;

namespace UnitTests
{
    [TestClass]
    public class UnitTestSquareList
    {
        private class SimpleComparable : IComparable
        {
            private readonly int value;
            public SimpleComparable(int val) { value = val; }
            public int CompareTo(object obj)
            {
                return ((SimpleComparable)obj).value - value;
            }
            public override string ToString()
            {
                return $"|{value}|";
            }
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
            Assert.AreEqual(6, sl.MaxDepth);
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
            Assert.AreEqual(5, sl.MaxDepth);
            sl.Delete(100);
            Assert.AreEqual(1, sl.Min);
            Assert.AreEqual(93, sl.Max);
            Assert.AreEqual(23, sl.Size);
            Assert.AreEqual(5, sl.MaxDepth);
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
            Assert.AreEqual(3, sl.MaxDepth);
            sl.Delete(1);
            sl.Delete(2);
            sl.Delete(3);
            sl.Delete(4);
            sl.Delete(5);
            Assert.AreEqual(6, sl.Min);
            Assert.AreEqual(9, sl.Max);
            Assert.AreEqual(4, sl.Size);
            Assert.AreEqual(2, sl.MaxDepth);
        }


    }
}
