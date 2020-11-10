Commons.Collections.SquareList
==============================

----------

Managed Commons library containing the SquareList specialized collection.

Loosely based on an [article](http://www.drdobbs.com/database/the-squarelist-data-structure/184405336) published in the May 2013 issue of Dr Dobb's Journal, by Mark Sams.
His implementation in C used circular doubly-linked lists of doubly-linked lists, I've reimplemented using a List of Dynamic Slices backed a single Big Array, It uses a lot less memory than linked lists, but inserts in crammed sets can have ripple effects with worst case O(n).

**This implementation allows for multiple instances of the same value to be inserted, and then deleted one-at-a-time or wholesale.**

**Thinking about performance, search for counting/existence is done bidirectionally, but removal scans only in the forward direction, to have a stable behavior (remove the first found) when duplicates exist.**

The SquareList is a structure that is particularly useful in applications that frequently require the current minimum and maximum values, as they both can be found in constant time, even accounting for deletions.
This implementation performs insert/delete/find operations within a worst-case running time of O(sqrt(n)) [In truth O(n) for inserts]. Values are always kept in ascending order, so traversing it in that order is natural and performant.

__Commons.Collections.SquareList 1.0.2 is available at Nuget:__ [Commons.Collections.SquareList](https://www.nuget.org/packages/Commons.Collections.SquareList/).

Performance
===

Some performance testing, all times in *ms* (updated):

SortedList (all times in *ms*)
---

|Initial Size| Creation |  Deletes | Inserts  |  Searchs |   Min    |   Max    |
|-----------:|---------:|---------:|---------:|---------:|---------:|---------:|
|        100 |        0 |        0 |        0 |        6 |       80 |       94 |
|        178 |        0 |        0 |        0 |        0 |       84 |       90 |
|        317 |        0 |        0 |        0 |        0 |       84 |       89 |
|        563 |        0 |        0 |        0 |        0 |       86 |       95 |
|       1000 |        0 |        0 |        0 |        0 |       84 |       96 |
|       1779 |        0 |        0 |        0 |        0 |       87 |       90 |
|       3163 |        0 |        0 |        0 |        0 |       80 |       89 |
|       5624 |        0 |        0 |        0 |        0 |       90 |       90 |
|      10000 |        0 |        0 |        0 |        0 |       82 |       91 |
|      17783 |        0 |        0 |        8 |        0 |       90 |      100 |
|      31623 |        0 |        0 |        0 |        0 |       84 |       99 |
|      56235 |       10 |        0 |        7 |        0 |       83 |       90 |
|     100000 |        9 |       11 |        7 |        0 |       84 |       90 |
|     177828 |       20 |       13 |        7 |        0 |       80 |       90 |
|     316228 |       39 |       31 |       30 |        0 |       89 |       87 |
|     562342 |       60 |      100 |      107 |        0 |       87 |       90 |
|    1000000 |      114 |      157 |      144 |        0 |       80 |       90 |
|    1778280 |      224 |      276 |      294 |        0 |       80 |       96 |
|    3162278 |      394 |      510 |      533 |        0 |       82 |       96 |
|    5623414 |      719 |      914 |      941 |        0 |       83 |       87 |
|   10000000 |     1353 |     1610 |     1607 |        0 |       93 |       90 |
|   17782795 |     2429 |     2981 |     2980 |        0 |       80 |       88 |
|   31622777 |     4564 |     5359 |     5330 |        0 |       86 |       94 |
|   56234133 |     8090 |     9460 |     9500 |        0 |       80 |       86 |

SquareList 
---
|Initial Size| Creation |  Deletes | Inserts  | Reinserts|  Searchs |   Min    |   Max    | CutInHalf| Shrink   |
|-----------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|
|        100 |        4 |       13 |        7 |        4 |        0 |        0 |        0 |        0 |        0 |
|        178 |        0 |        0 |        0 |        0 |        0 |        0 |        0 |        0 |        0 |
|        317 |        0 |        0 |        0 |        0 |        0 |        0 |        0 |        0 |        0 |
|        563 |        0 |        0 |        0 |        0 |        0 |        0 |        0 |        0 |        0 |
|       1000 |        0 |        0 |        0 |        0 |        7 |        0 |        0 |        0 |        0 |
|       1779 |        0 |        0 |        0 |        5 |        0 |        0 |        0 |        0 |        0 |
|       3163 |        0 |        4 |        0 |        0 |        0 |        0 |        0 |        0 |        0 |
|       5624 |        0 |        0 |        7 |        0 |        0 |        0 |        0 |        0 |        0 |
|      10000 |        0 |        0 |        0 |       10 |        0 |        0 |        0 |        0 |        0 |
|      17783 |        0 |        0 |        0 |        7 |        0 |        0 |        0 |        0 |        0 |
|      31623 |       10 |        0 |        0 |       17 |        0 |        0 |        0 |        0 |        0 |
|      56235 |        4 |        0 |        0 |       31 |        0 |        0 |        0 |        0 |        6 |
|     100000 |       10 |        0 |        4 |       50 |        0 |        0 |        0 |        0 |        7 |
|     177828 |       16 |        0 |        0 |       94 |        6 |        0 |        0 |        1 |        9 |
|     316228 |       30 |        0 |        0 |      174 |        0 |        0 |        0 |        7 |       14 |
|     562342 |       54 |        0 |        0 |      295 |        0 |        0 |        0 |        5 |       29 |
|    1000000 |      100 |        0 |        0 |      526 |        0 |        0 |        0 |        0 |       60 |
|    1778280 |      174 |        7 |        0 |      932 |        0 |        0 |        0 |       20 |       91 |
|    3162278 |      300 |       11 |        0 |     1718 |        0 |        0 |        0 |       30 |      161 |
|    5623414 |      544 |        7 |        0 |     2920 |        0 |        0 |        0 |       40 |      290 |
|   10000000 |      990 |        7 |        4 |     5216 |        0 |        0 |        0 |       90 |      500 |
|   17782795 |     1776 |       10 |        4 |     9337 |        0 |        0 |        0 |      123 |      909 |
|   31622777 |     3100 |       14 |       10 |    16708 |        0 |        0 |        0 |      212 |     1620 |
|   56234133 |     5449 |       17 |       10 |    29229 |        0 |        0 |        0 |      382 |     2874 |
	
----------

License: MIT 
------------
See LICENSE

