Commons.Collections.SquareList
==============================


----------

Managed Commons library containing the SquareList specialized collection.

Loosely based on an (article)(http://www.drdobbs.com/database/the-squarelist-data-structure/184405336) published in the May 2013 issue of Dr Dobb's Journal, by Mark Sams.
His implementation in C used circular doubly-linked lists of doubly-linked lists, I've reimplemented using a List of LinkedLists, which seemed more natural in C#, but that can surely be revisited.
This implementation allows for multiple instances of the same value to be inserted, and then deleted one-at-a-time or wholesale.
Thinking in performance, search for counting/existence is done bidirectionally, but removal scans only in the forward direction, to have stable behavior (remove the first found) when duplicates exist.


The SquareList is a structure that is particularly useful in applications that frequently require the current minimum and maximum values, as they both can be found in constant time, even accounting for deletions.
This structure in the original implementation performed insert/delete/find operations within a worst-case running time of O(sqrt(n)). It is always kept in ascending order, so traversing it in that order is natural and performant.

__Commons.Collections.SquareList 1.0.0-Beta is NOT YET available as a pre-release Nuget:__ [Commons.Collections.SquareList](https://www.nuget.org/packages/Commons.Core/).


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
