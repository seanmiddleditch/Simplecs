Simplecs - Simple ECS for C#
============================

http://github.com/seanmiddleditch/Simplecs

Simple Entity-Component-System library for C# with no dependencies on any game engine.

This library is not the fastest nor more powerful. It's useful for simple cases. More
sophisticated games or users should probably look elsewhere for a suitable library.
The intended use cases are small demos, toys, and examples.

This library uses a naive approach based on sparse sets to implement O(1) component
lookup and very cache-friendly iteration of component structures. The cost is a
some more memory lookup overhead than would be purely desired. Further, iterating sets
of components requires many of these lookup operations, as compared to iterating a
single component which is very cache-friendly in this architecture.

The concept of a System is actually not found in this library. A View can be used to
implement a System, but the scheduling and logic execution is up to the library user.
This library primarily implements a way of allocating Entity keys and storing Component
instances associated via these keys, along with a View concept to iterate Entities with
a matching set of Components..

This library does not attempt to provide any form of multi-threading, scheduling,
advanced debugging, serialization, or reflection. These are all up to the library user.
Feature additions that keep the library simple in nature may be considered in the future.

License
-------

Written in 2019 by Sean Middleditch (http://github.com/seanmiddleditch).

To the extent possible under law, the author(s) have dedicated all copyright
and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.

You should have received a copy of the CC0 Public Domain Dedication along
with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
