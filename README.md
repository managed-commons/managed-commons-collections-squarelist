Commons.Collections.SquareList
==============================

----------

Managed Commons library containing the SquareList specialized collection.

**Now requires .NET 6.0/C# 10 to build**

Loosely based on an [article](http://www.drdobbs.com/database/the-squarelist-data-structure/184405336) published in the May 2013 issue of Dr Dobb's Journal, by Mark Sams.
His implementation in C used circular doubly-linked lists of doubly-linked lists, I've reimplemented using a List of Dynamic Slices backed a single Big Array, It uses a lot less memory than linked lists, but inserts in crammed sets can have ripple effects with worst case O(n).

**This implementation allows for multiple instances of the same value to be inserted, and then deleted one-at-a-time or wholesale.**

**Thinking about performance, search for counting/existence is done bidirectionally, but removal scans only in the forward direction, to have a stable behavior (remove the first found) when duplicates exist.**

The SquareList is a structure that is particularly useful in applications that frequently require the current minimum and maximum values, as they both can be found in constant time, even accounting for deletions.
This implementation performs insert/delete/find operations within a worst-case running time of O(sqrt(n)) [In truth O(n) for inserts]. Values are always kept in ascending order, so traversing it in that order is natural and performant.

__Commons.Collections.SquareList 1.0.5 is available at Nuget:__ [Commons.Collections.SquareList](https://www.nuget.org/packages/Commons.Collections.SquareList/).

Targets in the package
---

|_Release_|_.NET Standard 1.0_|_.NET Standard 2.1_|_.NET 5.0_|_.NET 6.0_|
|--------:|------------------:|------------------:|---------:|---------:|
|   1.0.0 | ✔ |   |   |   |
|   1.0.2 |   | ✔ | ✔ |   |
|   1.0.3 |   | ✔ | ✔ | ✔ |
|   1.0.4 | ✔ | ✔ | ✔ | ✔ |
|   1.0.5 | ✔ | ✔ | ✔ | ✔ |

Performance
===

Some performance testing, all times in *ms* (updated and now using release version of nuget, on .NET 6.0):

SortedList (all times in *ms*) [.NET 6.0]
---

|Initial Size| Creation |  Deletes | Inserts  |  Searchs |   Min    |   Max    |
|-----------:|---------:|---------:|---------:|---------:|---------:|---------:|
|       1000 |        1 |        1 |        0 |        0 |       16 |       19 |
|       1779 |        0 |        0 |        0 |        0 |       15 |       19 |
|       3163 |        0 |        0 |        0 |        0 |       16 |       19 |
|       5624 |        0 |        0 |        0 |        0 |       15 |       13 |
|      10000 |        0 |        1 |        1 |        0 |        6 |        3 |
|      17783 |        1 |        1 |        2 |        0 |        3 |        3 |
|      31623 |        1 |        2 |        3 |        0 |        3 |        3 |
|      56235 |        2 |        4 |        5 |        0 |        3 |        3 |
|     100000 |        4 |        9 |       10 |        0 |        3 |        4 |
|     177828 |        7 |       16 |       19 |        0 |        3 |        4 |
|     316228 |       17 |       39 |       38 |        0 |        3 |        3 |
|     562342 |       26 |      188 |      115 |        0 |        3 |        3 |
|    1000000 |       47 |      363 |      127 |        0 |        2 |        3 |
|    1778280 |       77 |      575 |      302 |        0 |        3 |        3 |
|    3162278 |      144 |     1050 |      809 |        0 |        3 |        3 |
|    5623414 |      261 |     1893 |     1528 |        0 |        3 |        3 |
|   10000000 |      484 |     3421 |     2930 |        0 |        3 |        2 |
|   17782795 |      874 |     6045 |     5311 |        0 |        3 |        3 |
|   31622777 |     1598 |    10743 |     9482 |        1 |        3 |        3 |


SquareList 
---

|Initial Size| Creation |  Deletes | Inserts  | Reinserts|  Searchs |   Min    |   Max    | CutInHalf| Shrink   |
|-----------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|
|       1000 |        2 |        3 |        2 |        3 |        0 |        0 |        0 |        1 |        1 |
|       1779 |        0 |        0 |        1 |        0 |        0 |        0 |        0 |        0 |        0 |
|       3163 |        0 |        1 |        1 |        0 |        0 |        0 |        0 |        0 |        0 |
|       5624 |        0 |        1 |        1 |        0 |        0 |        0 |        0 |        0 |        0 |
|      10000 |        0 |        1 |        1 |        1 |        0 |        0 |        0 |        0 |        0 |
|      17783 |        0 |        1 |        1 |        1 |        0 |        0 |        0 |        0 |        0 |
|      31623 |        0 |        1 |        1 |        2 |        0 |        0 |        0 |        0 |        0 |
|      56235 |        1 |        1 |        1 |        3 |        0 |        0 |        0 |        0 |        0 |
|     100000 |        1 |        2 |        1 |        5 |        0 |        0 |        0 |        0 |        1 |
|     177828 |        2 |        3 |        2 |       10 |        0 |        0 |        0 |        0 |        1 |
|     316228 |        6 |        3 |        3 |       19 |        0 |        0 |        0 |        0 |        2 |
|     562342 |       11 |        4 |        4 |       31 |        0 |        0 |        0 |        1 |        5 |
|    1000000 |       15 |        2 |        1 |       51 |        0 |        0 |        0 |        0 |        6 |
|    1778280 |       27 |        6 |        5 |       83 |        0 |        0 |        0 |        2 |       10 |
|    3162278 |       45 |        8 |        8 |      159 |        1 |        0 |        0 |        3 |       18 |
|    5623414 |       72 |        9 |        8 |      287 |        1 |        0 |        0 |        4 |       34 |
|   10000000 |      129 |       12 |       10 |      500 |        1 |        0 |        0 |        9 |       60 |
|   17782795 |      237 |       17 |       14 |      890 |        1 |        0 |        0 |       14 |      108 |
|   31622777 |      412 |       21 |       18 |     1584 |        1 |        0 |        0 |       24 |      189 |

----------

License: MIT 
------------
See [LICENSE](LICENSE)

