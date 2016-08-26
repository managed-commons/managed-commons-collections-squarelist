Commons.Collections.SquareList
==============================

----------

Managed Commons library containing the SquareList specialized collection.

Loosely based on an [article](http://www.drdobbs.com/database/the-squarelist-data-structure/184405336) published in the May 2013 issue of Dr Dobb's Journal, by Mark Sams.
His implementation in C used circular doubly-linked lists of doubly-linked lists, I've reimplemented using a List of LinkedLists, which seemed more natural in C#, but that can surely be revisited.

**This implementation allows for multiple instances of the same value to be inserted, and then deleted one-at-a-time or wholesale.**

**Thinking about performance, search for counting/existence is done bidirectionally, but removal scans only in the forward direction, to have a stable behavior (remove the first found) when duplicates exist.**

The SquareList is a structure that is particularly useful in applications that frequently require the current minimum and maximum values, as they both can be found in constant time, even accounting for deletions.
This implementation performs insert/delete/find operations within a worst-case running time of O(sqrt(n)). Values are always kept in ascending order, so traversing it in that order is natural and performant.

__Commons.Collections.SquareList 1.0.0-Beta is NOT YET available as a pre-release Nuget:__ [Commons.Collections.SquareList](https://www.nuget.org/packages/Commons.Core/).


Some performance testing:

    ------------------------------------------------------------------------------------------------------------------------
    |              |              SortedList                           |              SquareList                           |
    | Initial Size |  Creation  |  Inserts   |   Deletes  |   Searchs  |  Creation  |  Inserts   |   Deletes  |   Searchs  |
    |  (elements)  |    (ms)    |    (ms)    |    (ms)    |    (ms)    |    (ms)    |    (ms)    |    (ms)    |    (ms)    |
    ------------------------------------------------------------------------------------------------------------------------
    |         100  |          2 |          1 |          2 |          0 |          5 |          4 |          3 |          2 |
    |         178  |          0 |          0 |          0 |          0 |          0 |          0 |          1 |          0 |
    |         317  |          0 |          0 |          0 |          0 |          0 |          0 |          1 |          0 |
    |         563  |          0 |          0 |          0 |          0 |          0 |          0 |          0 |          1 |
    |        1000  |          0 |          0 |          0 |          0 |          0 |          1 |          0 |          0 |
    |        1779  |          0 |          0 |          0 |          0 |          1 |          0 |          1 |          0 |
    |        3163  |          0 |          1 |          0 |          0 |          1 |          0 |          1 |          0 |
    |        5624  |          0 |          0 |          0 |          0 |         29 |          0 |          1 |          1 |
    |       10000  |          1 |          0 |          0 |          0 |          4 |          1 |          1 |          0 |
    |       17783  |          2 |          1 |          0 |          0 |          9 |          1 |          2 |          0 |
    |       31623  |          4 |          1 |          1 |          0 |         13 |         10 |          2 |          0 |
    |       56235  |          5 |          2 |          2 |          0 |         33 |          2 |          3 |          0 |
    |      100000  |         11 |          3 |          3 |          0 |         59 |          2 |          3 |          1 |
    |      177828  |         20 |          6 |          4 |          0 |        102 |          3 |         23 |          1 |
    |      316228  |         42 |          9 |         11 |          0 |        201 |         18 |          7 |          1 |
    |      562342  |         86 |         31 |         34 |          0 |        386 |          6 |         17 |          2 |
    |     1000000  |        120 |         53 |         49 |          0 |        656 |          9 |         21 |          2 |
    |     1778280  |        203 |        133 |        125 |          0 |       1242 |         24 |         20 |          2 |
    |     3162278  |        374 |        254 |        245 |          0 |       2369 |         13 |         50 |          3 |
    |     5623414  |        674 |        458 |        448 |          0 |       4342 |         19 |         31 |          5 |
    |    10000000  |       1243 |        831 |        802 |          0 |       7610 |         33 |         43 |          3 |
    |    17782795  |       2273 |       1552 |       1463 |          0 |      13432 |         64 |         59 |          5 |
    |    31622777  |       4361 |       2703 |       2585 |          1 |      27323 |        128 |        199 |         27 |
    |    56234133  |       8171 |       4815 |       4684 |          0 |      54359 |        181 |        298 |         30 |
    ------------------------------------------------------------------------------------------------------------------------

----------

License: MIT
------------

Copyright (c) 2016 Rafael 'Monoman' Teixeira, Managed Commons Team

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

