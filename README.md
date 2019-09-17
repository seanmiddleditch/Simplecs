Simplecs - Simple ECS for C#
============================

http://github.com/seanmiddleditch/Simplecs

Simple Entity-Component-System library for C# with no dependencies on any game engine.

Pronounced "simplex."

This library is not the fastest nor most powerful. It's useful for simple cases. More
sophisticated games or users may need to look elsewhere for a suitable library.
The intended use cases are small demos, toys, and examples.

This library uses a naive approach based on sparse sets to implement O(1) component
lookup. Iteration of a component table is cache-friendly, but iteration of a `View`
over multiple components or using required/excluded components will incur non-cache-
friendly memory access patterns.

The ECS concept of a "System" is actually not found in this library. A `View` can be used
to implement a System, but the scheduling and logic execution is up to the library user.
This library primarily implements a way of allocating `Entity` keys and storing Component
instances associated via these keys, along with a `View` concept to iterate Entities with
a matching set of Components.

A Component in Simplecs is any `struct` type that is `Attach`ed to an `Entity`. There are
no other restrictions or requirements on Components.

This library does not attempt to provide any form of multi-threading, scheduling,
advanced debugging, serialization, or reflection. These are all up to the library user.
Feature additions that keep the library simple in nature may be considered in the future.

Usage
-----

The `World` class is the main container that stores all entities and components.

```c#
var world = new World();
```

Component types _must_ be `struct` types in Simplecs.

```c#
struct PositionComponent {
    public int x, y z;
}
```

Entities are identified by the `Entity` type, which is just an opaque handle. An entity
can be constructed on a `World` using a builder interface, which can also be used to
attach components.

```c#
var entity = world.CreateEntity()
    .Attach(new PositionComponent{x = 1, y = -2, z = 0})
    .Entity;
```

Components can also be attached and detached after creation.

```c#
world.Attach(entity, new NameComponent("Bob"));
world.Detach<PositionComponent>(entity);
```

Entities can be destroyed, which detaches all components. Unused entities should be
destroyed to reclaim memory.

```c#
world.Destroy(entity);
```

Querying entities uses a `View<>` generic, which can be constructed via a builder on
`World` like entities.

```c#
var view = world.CreateView().Select<PositionComponent, NameComponent>();
```

Views can be iterated to retrieve a list of entities and references to the selected
components. Note that these references are fully mutable, which can be handy for some
kinds of frequently-modified heavy components.

```c#
var view = world.CreateView().Select<PositionComponent, NameComponent>();
foreach (var row in view) {
    Console.WriteLine(row.Component2.Name);
}
```

A `View` can also be constructed with required components or excluded components.
These components are used to find matching entities, but are not included in the
iterated list of components like `Select<>` does. Excluded components are those which
must not exist on an entity for it to match. Note that `Select<>` must be the final
method in the builder chain.

```c#
// select all entities with a position, and which also
// have an actor component but do not have an AI component
//
var view = world.CreateView()
    .Require<ActorComponent>()
    .Exclude<AIComponent>()
    .Select<PositionComponent>();

// move all non-AI actors slightly to the right
//
foreach (var row in view) {
    row.Component.x += 1;
}
```

Additional support in the public interface exists to query which entities exist
and to find all components on a given entity, which can be useful for tooling or
serialization. Note that some of these interfaces necessitate boxing components
which can be a significant performance penalty.

License
-------

Written in 2019 by Sean Middleditch (http://github.com/seanmiddleditch).

To the extent possible under law, the author(s) have dedicated all copyright
and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.

You should have received a copy of the CC0 Public Domain Dedication along
with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
