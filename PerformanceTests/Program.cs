using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Collections;
using static System.Console;

namespace PerformanceTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WriteLine("Performance tests");

            WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            WriteLine("|============|              SortedList                                         |              SquareList                                                                          |");
            WriteLine("|Initial Size| Creation |  Deletes | Inserts  |  Searchs |   Min    |   Max    | Creation |  Deletes | Inserts  | Reinserts|  Searchs |   Min    |   Max    | CutInHalf| Shrink   |");
            WriteLine("| (elements) |   (ms)   |   (ms)   |   (ms)   |   (ms)   |   (ms)   |   (ms)   |   (ms)   |   (ms)   |   (ms)   |  (ms)    |   (ms)   |   (ms)   |   (ms)   |  (ms)    |  (ms)    |");
            WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            var multiplier = Math.Sqrt(Math.Sqrt(10d));
            for (double sizef = 100d; sizef <= 60000000d; sizef *= multiplier) {
                int size = (int)Math.Ceiling(sizef);
                var r = RunTestsFor(size);
                WriteLine($"| {Pad(size, 10)} | {Pad(r.slCre)} | {Pad(r.slDel)} | {Pad(r.slIns)} | {Pad(r.slSearch)} | {Pad(r.slMin)} | {Pad(r.slMax)} | {Pad(r.sqlCre)} | {Pad(r.sqlDel)} | {Pad(r.sqlIns)} | {Pad(r.sqlDupIns)} | {Pad(r.sqlSearch)} | {Pad(r.sqlMin)} | {Pad(r.sqlMax)} | {Pad(r.sqlCutInHalf)} | {Pad(r.sqlShrink)} |");
            }
            WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            ReadLine();
        }

        private static void Create(Action<int> action, int size)
        {
            for (int i = 1; i <= size; i++)
                action(i);
        }

        private static void CutInHalf(Action<int> action, int size)
        {
            size /= 2;
            for (int i = 1; i <= size; i++)
                action(i);
        }

        private static void HundredTimes(Action<int> action, int size)
        {
            int value = 1;
            int increment = size / 100;
            for (int i = 1; i <= 100; i++) {
                action(value);
                value += increment;
            }
        }

        private static void MillionTimes(Action action)
        {
            for (int i = 1; i <= 1000000; i++) {
                action();
            }
        }

        private static string Pad(int value, int length = 8) => value.ToString("0").PadLeft(length);

        private static IEnumerable<int> Range(int size)
        {
            for (int i = 1; i <= size; i++)
                yield return i;
        }

        private static Results RunTestsFor(int size)
        {
            var results = new Results(size);
            int dummy = 0;
            var sl = new SortedList<int, int>(size);
            results.slCre = Time(() => Create((i) => sl.Add(i, i), size));
            results.slDel = Time(() => HundredTimes((i) => sl.Remove(i), size));
            results.slIns = Time(() => HundredTimes((i) => sl.Add(i, i), size));
            results.slSearch = Time(() => HundredTimes((i) => sl.ContainsKey(i), size));
            results.slMin = Time(() => MillionTimes(() => dummy = sl.Keys[0]));
            results.slMax = Time(() => MillionTimes(() => dummy = sl.Keys[sl.Keys.Count - 1]));
            SquareList<int> sql = null;
            results.sqlCre = Time(() => sql = new SquareList<int>(size, Range(size)));
            if (!sql.Take(10).SequenceEqual(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }))
                throw new InvalidOperationException();
            results.sqlDel = Time(() => HundredTimes((i) => sql.Delete(i), size));
            if (sql.Size != size - 100)
                throw new InvalidOperationException();
            results.sqlIns = Time(() => HundredTimes((i) => sql.Insert(i), size));
            if (!sql.Take(10).SequenceEqual(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }))
                throw new InvalidOperationException();
            results.sqlDupIns = Time(() => HundredTimes((i) => sql.Insert(i), size));
            if (!sql.Take(2).SequenceEqual(new int[] { 1, 1 }))
                throw new InvalidOperationException();
            results.sqlSearch = Time(() => HundredTimes((i) => sql.Contains(i), size));
            results.sqlCutInHalf = Time(() => sql.DeleteBelow(size / 2));
            results.sqlShrink = Time(() => sql.ShrinkWithSlackOf(0));
            if (sql.Min != (size / 2))
                throw new InvalidOperationException();
            results.slMin = Time(() => MillionTimes(() => dummy = sql.Min));
            if (sql.Max != size)
                throw new InvalidOperationException();
            results.slMax = Time(() => MillionTimes(() => dummy = sql.Max));

            return results;
        }

        private static int Time(Action action)
        {
            var start = DateTimeOffset.UtcNow;
            try { action(); } catch (Exception e) { WriteLine(e); }
            return Convert.ToInt32((DateTimeOffset.UtcNow - start).TotalMilliseconds);
        }

        private struct Results
        {
            public int size, slCre, slIns, slDel, slSearch, sqlCre, sqlIns, sqlDel, sqlSearch;
            public int sqlDupIns, sqlCutInHalf, sqlShrink, slMin, slMax, sqlMin, sqlMax;

            public Results(int size)
            {
                this.size = size;
                slCre = slIns = slDel = slSearch = sqlCre = sqlIns = sqlDel = sqlSearch = sqlDupIns = 0;
                sqlCutInHalf = sqlShrink = slMin = slMax = sqlMin = sqlMax = 0;
            }
        }
    }
}
