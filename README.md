Commons.Collections.SquareList
==============================

----------

Managed Commons library containing the SquareList specialized collection.

**Now requires .NET 8.0/C# 12 to build**

Loosely based on an [article](http://www.drdobbs.com/database/the-squarelist-data-structure/184405336) published in the May 2013 issue of Dr Dobb's Journal, by Mark Sams.
His implementation in C used circular doubly-linked lists of doubly-linked lists, I've reimplemented using a List of Dynamic Slices backed by a single Big Array, It uses a lot less memory than linked lists, but inserts in crammed sets can have ripple effects with worst case O(n).

**This implementation allows for multiple instances of the same value to be inserted, and then deleted one-at-a-time or wholesale.**

**Thinking about performance, search for counting/existence is done bidirectionally, but removal scans only in the forward direction, to have a stable behavior (remove the first found) when duplicates exist.**

The SquareList is a structure that is particularly useful in applications that frequently require the current minimum and maximum values, as they both can be found in constant time, even accounting for deletions.
This implementation performs insert/delete/find operations within a worst-case running time of O(sqrt(n)) [In truth O(n) for inserts]. Values are always kept in ascending order, so traversing it in that order is natural and performant.

__Commons.Collections.SquareList 1.1.1 is available at Nuget:__ [Commons.Collections.SquareList](https://www.nuget.org/packages/Commons.Collections.SquareList/).

Targets in the package
---

|_Release_|_.NET Standard 1.0_|_.NET Standard 2.1_|_.NET 6.0_|_.NET 8.0_|_.NET 9.0_|
|--------:|------------------:|------------------:|---------:|---------:|---------:|
|   1.0.0 | ✔ |   |   |   | |
|   1.0.2 |    | ✔ |   |   | |
|   1.0.3 |    | ✔ | ✔ |   | |
|   1.0.4 | ✔ | ✔ | ✔ |   | |
|   1.0.5 | ✔ | ✔ | ✔ |   | |
|   1.0.6 | ✔ | ✔ | ✔ |   | |
|   1.0.7 | ✔ | ✔ | ✔ | ✔ | |
|   1.0.8 | ✔ | ✔ | ✔ | ✔ | |
|   1.1.0 | ✘ | ✔ | ✘| ✔ | ✔ |

Performance
===

Some performance testing, all times in *ms* (updated and now using release version of nuget, on .NET 6.0):

SortedList (all times in *ms*) [.NET 8.0]                                       
---                                                                             

|Initial Size| Creation |  Deletes | Inserts  |  Searchs |   Min    |   Max    |
|-----------:|---------:|---------:|---------:|---------:|---------:|---------:|
|       1000 |        0 |        1 |        0 |        0 |       26 |       34 |
|       1779 |        0 |        0 |        0 |        0 |       24 |       28 |
|       3163 |        0 |        0 |        0 |        0 |       32 |       37 |
|       5624 |        0 |        1 |        1 |        0 |       24 |       32 |
|      10000 |        1 |        2 |       16 |        0 |       23 |       29 |
|      17783 |        1 |        1 |        2 |        0 |       26 |       29 |
|      31623 |        2 |        3 |        4 |        0 |       20 |       30 |
|      56235 |        4 |        5 |        5 |        0 |       26 |       44 |
|     100000 |       20 |       27 |       26 |        0 |       28 |       34 |
|     177828 |       11 |       19 |       25 |        0 |       36 |       29 |
|     316228 |       20 |       37 |       38 |        0 |       23 |       30 |
|     562342 |       37 |      139 |       58 |        0 |       20 |       26 |
|    1000000 |       60 |      266 |      121 |        0 |       20 |       25 |
|    1778280 |      109 |      540 |      296 |        0 |       20 |       25 |
|    3162278 |      198 |     1001 |      728 |        0 |       20 |       25 |
|    5623414 |      402 |     1804 |     1480 |        0 |       19 |       25 |
|   10000000 |      653 |     3237 |     2806 |        0 |       20 |       27 |
|   17782795 |     1195 |     5789 |     5294 |        0 |       21 |       27 |
|   31622777 |     2235 |    10510 |     9456 |        1 |       20 |       27 |

SquareList (all times in *ms*) [.NET 8.0]
---

|Initial Size| Creation |  Deletes | Inserts  | Reinserts|  Searchs |   Min    |   Max    | CutInHalf| Shrink   |
|-----------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|
|       1000 |        3 |        2 |        1 |        1 |        1 |        0 |        0 |        0 |        0 |
|       1779 |        0 |        1 |        2 |        0 |        1 |        0 |        0 |        0 |        0 |
|       3163 |        2 |        1 |        1 |        1 |        2 |        0 |        0 |        0 |        0 |
|       5624 |        0 |        1 |        1 |        1 |        1 |        0 |        0 |        0 |        0 |
|      10000 |        1 |        3 |        2 |        3 |        1 |        0 |        0 |        0 |        0 |
|      17783 |        1 |        4 |        3 |        3 |        1 |        0 |        0 |        0 |        0 |
|      31623 |        2 |        4 |        3 |        4 |        1 |        0 |        0 |        0 |        1 |
|      56235 |        3 |        6 |        4 |       10 |        1 |        0 |        0 |        0 |        1 |
|     100000 |        5 |        6 |        4 |       12 |        1 |        0 |        0 |        0 |        9 |
|     177828 |        9 |        7 |        5 |       23 |        1 |        0 |        0 |        1 |        3 |
|     316228 |       17 |        9 |        7 |       39 |        1 |        0 |        0 |        1 |        6 |
|     562342 |       25 |       10 |        8 |       60 |        1 |        0 |        0 |        2 |        9 |
|    1000000 |       54 |       12 |        4 |      107 |        1 |        0 |        0 |        1 |       16 |
|    1778280 |       78 |       17 |       16 |      183 |        1 |        0 |        0 |        5 |       30 |
|    3162278 |      143 |       22 |       17 |      332 |        2 |        0 |        0 |       13 |       59 |
|    5623414 |      249 |       28 |       22 |      594 |        2 |        0 |        0 |       12 |       87 |
|   10000000 |      442 |       36 |       31 |     1052 |        2 |        0 |        0 |       35 |      157 |
|   17782795 |      790 |       50 |       41 |     1890 |        2 |        0 |        0 |       45 |      287 |
|   31622777 |     1412 |       66 |       55 |     3358 |        2 |        0 |        0 |       80 |      501 |

----------

License: MIT 
------------
See [LICENSE](LICENSE)

