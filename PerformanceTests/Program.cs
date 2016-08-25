using System;
using System.Collections.Generic;
using Commons.Collections;
using static System.Console;

namespace PerformanceTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WriteLine("Performance tests");

            WriteLine("------------------------------------------------------------------------------------------------------------------------");
            WriteLine("|==============|              SortedList                           |              SquareList                           |");
            WriteLine("| Initial Size |  Creation  |  Inserts   |   Deletes  |   Searchs  |  Creation  |  Inserts   |   Deletes  |   Searchs  |");
            WriteLine("|  (elements)  |    (ms)    |    (ms)    |    (ms)    |    (ms)    |    (ms)    |    (ms)    |    (ms)    |    (ms)    |");
            WriteLine("------------------------------------------------------------------------------------------------------------------------");
            var multiplier = Math.Sqrt(Math.Sqrt(10d));
            for (double sizef = 100d; sizef <= 60000000d; sizef *= multiplier) {
                int size = (int)Math.Ceiling(sizef);
                var r = RunTestsFor(size);
                WriteLine($"|  {Pad(size)}  | {Pad(r.slCre)} | {Pad(r.slIns)} | {Pad(r.slDel)} | {Pad(r.slSearch)} | {Pad(r.sqlCre)} | {Pad(r.sqlIns)} | {Pad(r.sqlDel)} | {Pad(r.sqlSearch)} |");
            }
            WriteLine("------------------------------------------------------------------------------------------------------------------------");
            ReadLine();
        }

        private static void Create(Action<int> action, int size)
        {
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

        private static string Pad(int value, int length = 10) => value.ToString("0").PadLeft(length);

        private static Results RunTestsFor(int size)
        {
            var results = new Results(size);
            var sl = new SortedList<int, int>(size);
            results.slCre = Time(() => Create((i) => sl.Add(i, i), size));
            results.slDel = Time(() => HundredTimes((i) => sl.Remove(i), size));
            results.slIns = Time(() => HundredTimes((i) => sl.Add(i, i), size));
            results.slSearch = Time(() => HundredTimes((i) => sl.ContainsKey(i), size));

            var sql = new SquareList<int>(size + 100);
            results.sqlCre = Time(() => Create((i) => sql.Insert(i), size));
            results.sqlIns = Time(() => HundredTimes((i) => sql.Insert(i), size));
            results.sqlSearch = Time(() => HundredTimes((i) => sql.Contains(i), size));
            results.sqlDel = Time(() => HundredTimes((i) => sql.Delete(i), size));

            return results;
        }

        private static int Time(Action action)
        {
            var start = DateTimeOffset.UtcNow;
            try { action(); } catch (Exception) { }
            return Convert.ToInt32((DateTimeOffset.UtcNow - start).TotalMilliseconds);
        }

        private struct Results
        {
            public int size, slCre, slIns, slDel, slSearch, sqlCre, sqlIns, sqlDel, sqlSearch;

            public Results(int size)
            {
                this.size = size;
                slCre = slIns = slDel = slSearch = sqlCre = sqlIns = sqlDel = sqlSearch = 0;
            }
        }
    }
}
