# Commons.Collections.SquareList

----------

Managed Commons library containing the SquareList specialized collection.

> [!NOTE]
> **Now requires .NET 8.0/C# 12 to build**

Loosely based on an [article](http://www.drdobbs.com/database/the-squarelist-data-structure/184405336) published in the May 2013 issue of Dr Dobb's Journal, by Mark Sams.

His implementation in C used circular doubly-linked lists of doubly-linked lists, I've reimplemented using a List of Dynamic Slices backed by a single Big Array, It uses a lot less memory than linked lists, but inserts in crammed sets can have ripple effects with worst case O(n).

**This implementation allows for multiple instances of the same value to be inserted, and then deleted one-at-a-time or wholesale.**

**Thinking about performance, search for counting/existence is done bidirectionally, but removal scans only in the forward direction, to have a stable behavior (remove the first found) when duplicates exist.**

The SquareList is a structure that is particularly useful in applications that frequently require the current minimum and maximum values, as they both can be found in constant time, even accounting for deletions.

This implementation performs insert/delete/find operations within a worst-case running time of O(sqrt(n)) [In truth O(n) for inserts]. Values are always kept in ascending order, so traversing it in that order is natural and performant.

__Commons.Collections.SquareList 1.1.4 is available at Nuget:__ [Commons.Collections.SquareList](https://www.nuget.org/packages/Commons.Collections.SquareList/).

---
## Targets in the package

| _Release_ | _.NET Standard 1.0_ | _.NET Standard 2.1_ | _.NET 6.0_ | _.NET 8.0_ | _.NET 9.0_ | _.NET 10.0_ |
| --------: | :-----------------: | :-----------------: | :--------: | :--------: | :--------: | :---------: |
|     1.0.0 |          ✔          |                     |            |            |            |             |
|     1.0.2 |                     |          ✔          |            |            |            |             |
|     1.0.3 |                     |          ✔          |     ✔      |            |            |             |
|     1.0.4 |          ✔          |          ✔          |     ✔      |            |            |             |
|     1.0.5 |          ✔          |          ✔          |     ✔      |            |            |             |
|     1.0.6 |          ✔          |          ✔          |     ✔      |            |            |             |
|     1.0.7 |          ✔          |          ✔          |     ✔      |     ✔      |            |             |
|     1.0.8 |          ✔          |          ✔          |     ✔      |     ✔      |            |             |
|     1.1.0 |          ✘          |          ✔          |     ✘      |     ✔      |     ✔      |             |
|     1.1.3 |          ✘          |          ✔          |     ✘      |     ✔      |     ✔      |      ✔      |
|     1.1.4 |          ✘          |          ✔          |     ✘      |     ✔      |     ✔      |      ✔      |

---
## Performance

> [!Version Information]
> - Runtime: .NET 10.0.7
> - SortedList: 10.0.0
> - SquareList: 1.1.4

### Times (all in *ms*)

#### SortedList                                                                    

|Initial Size| Creation |  Deletes | Inserts  |  Searchs |   Min    |   Max    |   
|-----------:|---------:|---------:|---------:|---------:|---------:|---------:|   
|       1000 |        1 |        1 |        0 |        0 |       17 |       20 |   
|       1779 |        0 |        0 |        0 |        0 |       16 |       20 |   
|       3163 |        0 |        0 |        0 |        0 |       16 |       20 |   
|       5624 |        1 |        0 |        0 |        0 |        5 |        4 |   
|      10000 |        0 |        1 |        1 |        0 |        3 |        3 |   
|      17783 |        0 |        1 |        2 |        0 |        3 |        3 |   
|      31623 |        1 |        2 |        3 |        0 |        3 |        3 |   
|      56235 |        1 |        5 |        5 |        0 |        3 |        3 |   
|     100000 |        2 |        9 |       10 |        0 |        3 |        3 |   
|     177828 |        4 |       16 |       19 |        0 |        3 |        3 |   
|     316228 |        8 |       29 |       33 |        0 |        3 |        3 |   
|     562342 |       15 |      123 |       60 |        0 |        3 |        3 |   
|    1000000 |       28 |      288 |      121 |        0 |        3 |        3 |   
|    1778280 |       49 |      572 |      313 |        0 |        3 |        3 |   
|    3162278 |       94 |     1116 |      760 |        0 |        3 |        3 |   
|    5623414 |      170 |     1952 |     1548 |        0 |        3 |        3 |   
|   10000000 |      327 |     3544 |     2920 |        0 |        3 |        3 |   
|   17782795 |      593 |     6197 |     5274 |        0 |        2 |        3 |   
|   31622777 |     1070 |    11219 |     9748 |        1 |        3 |        3 |   

#### SquareList

|Initial Size| Creation |  Deletes | Inserts  | Reinserts|  Searchs |   Min    |   Max    | CutInHalf| Shrink   |
|-----------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|
|       1000 |        2 |        3 |        2 |        2 |        1 |        0 |        0 |        0 |        0 |
|       1779 |        0 |        7 |        4 |        1 |        1 |        0 |        0 |        0 |        0 |
|       3163 |        0 |        1 |        1 |        1 |        1 |        0 |        0 |        0 |        0 |
|       5624 |        3 |        1 |        1 |        1 |        1 |        0 |        0 |        0 |        0 |
|      10000 |        0 |        2 |        1 |        1 |        0 |        0 |        0 |        0 |        0 |
|      17783 |        0 |        1 |        1 |        2 |        0 |        0 |        0 |        0 |        2 |
|      31623 |        0 |        1 |        1 |        2 |        0 |        0 |        0 |        0 |        1 |
|      56235 |        0 |        1 |        2 |        3 |        0 |        0 |        0 |        0 |        1 |
|     100000 |        1 |        2 |        2 |        4 |        0 |        0 |        0 |        0 |        2 |
|     177828 |        1 |        2 |        3 |        7 |        0 |        0 |        0 |        0 |        4 |
|     316228 |        4 |        3 |        4 |       12 |        0 |        0 |        0 |        1 |        6 |
|     562342 |        4 |        2 |        2 |       27 |        0 |        0 |        0 |        0 |        2 |
|    1000000 |        8 |        1 |        6 |       47 |        0 |        0 |        0 |        0 |        3 |
|    1778280 |       13 |        3 |        3 |       84 |        0 |        0 |        0 |        1 |        6 |
|    3162278 |       23 |        5 |        4 |      156 |        0 |        0 |        0 |        2 |       10 |
|    5623414 |       41 |        6 |        5 |      278 |        1 |        0 |        0 |        2 |       18 |
|   10000000 |       74 |        7 |        7 |      665 |        1 |        0 |        0 |        5 |       33 |
|   17782795 |      124 |       11 |        9 |      888 |        1 |        0 |        0 |       12 |       51 |
|   31622777 |      232 |       14 |       12 |     1614 |        1 |        0 |        0 |       12 |       99 |

----------

## License: MIT 

See [LICENSE](LICENSE)

