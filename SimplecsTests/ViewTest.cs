using NUnit.Framework;
using Simplecs;
using System;
using System.Linq;

namespace SimplecsTests {
    public class ViewTest {
        [Test]
        public void EachMutable() {
            var world = new World();
            world.CreateEntity().Attach(new IntComponent { x = 1 });
            world.CreateEntity().Attach(new IntComponent { x = 2 });
            world.CreateEntity().Attach(new IntComponent { x = 3 });

            var view = world.CreateView().Select<IntComponent>();
            foreach (var row in view) {
                row.Component.x *= row.Component.x;
            }

            Assert.AreEqual(expected: 1, actual: view.ElementAt(0).Component.x);
            Assert.AreEqual(expected: 4, actual: view.ElementAt(1).Component.x);
            Assert.AreEqual(expected: 9, actual: view.ElementAt(2).Component.x);
        }

        [Test]
        public void Require() {
            var world = new World();
            world.CreateEntity().Attach(new IntComponent { x = 1 });
            world.CreateEntity().Attach(new NameComponent { name = "Bob" }).Attach(new IntComponent { x = 2 });
            world.CreateEntity().Attach(new IntComponent { x = 3 });
            world.CreateEntity().Attach(new NameComponent { name = "Susan" }).Attach(new IntComponent { x = 4 });

            var view = world.CreateView().Require<NameComponent>().Select<IntComponent>();

            Assert.AreEqual(expected: 2, actual: view.ElementAt(0).Component.x);
            Assert.AreEqual(expected: 4, actual: view.ElementAt(1).Component.x);
        }

        [Test]
        public void Exclude() {
            var world = new World();
            world.CreateEntity().Attach(new NameComponent { name = "Bob" }).Attach(new IntComponent { x = 2 });
            world.CreateEntity().Attach(new IntComponent { x = 1 });
            world.CreateEntity().Attach(new NameComponent { name = "Susan" }).Attach(new IntComponent { x = 4 });
            world.CreateEntity().Attach(new IntComponent { x = 3 });

            var view = world.CreateView().Exclude<NameComponent>().Select<IntComponent>();

            Assert.AreEqual(expected: 1, actual: view.ElementAt(0).Component.x);
            Assert.AreEqual(expected: 3, actual: view.ElementAt(1).Component.x);
        }

        [Test]
        public void Match() {
            var world = new World();
            var entity = world.CreateEntity()
                .Attach(new NameComponent { name = "Bob" })
                .Attach(new IntComponent { x = 7 })
                .Entity;

            world.CreateEntity()
                .Attach(new NameComponent { name = "Susan" })
                .Attach(new IntComponent { x = 90 });

            var nameView = world.CreateView().Select<NameComponent>();
            var intView = world.CreateView().Select<IntComponent>();
            var bothView = world.CreateView().Select<NameComponent, IntComponent>();

            Assert.IsTrue(nameView.Any());
            Assert.IsTrue(intView.Any());
            Assert.IsTrue(bothView.Any());

            Assert.AreEqual(expected: new NameComponent { name = "Bob" }, actual: nameView.FirstOrDefault().Component);
            Assert.AreEqual(expected: new IntComponent { x = 7 }, actual: intView.FirstOrDefault().Component);

            Assert.AreEqual(expected: new NameComponent { name = "Bob" }, actual: bothView.FirstOrDefault().Component1);
            Assert.AreEqual(expected: new IntComponent { x = 7 }, actual: bothView.FirstOrDefault().Component2);
        }

        [Test]
        public void Access() {
            var world = new World();
            var entity1 = world.CreateEntity()
                .Attach(new NameComponent { name = "Bob" })
                .Attach(new IntComponent { x = 7 })
                .Entity;

            var entity2 = world.CreateEntity()
                .Attach(new NameComponent { name = "Susan" })
                .Attach(new IntComponent { x = 90 })
                .Entity;

            var entity3 = world.CreateEntity()
                .Attach(new IntComponent { x = -1 })
                .Entity;

            var view = world.CreateView().Select<IntComponent, NameComponent>();

            Assert.IsTrue(view.Contains(entity1));
            Assert.IsTrue(view.Contains(entity2));
            Assert.IsFalse(view.Contains(entity3));

            Assert.IsTrue(view.TryGet(entity1, out var binding1));
            Assert.IsTrue(view.TryGet(entity2, out var binding2));
            
            Assert.AreEqual(expected: 7, actual: binding1.Component1.x);
            Assert.AreEqual(expected: 90, actual: binding2.Component1.x);
        }

        // [Test]
        // public void Invalidate() {
        //     var world = new World();
        //     world.CreateEntity().Attach(new IntComponent { x = 7 });
        //     var entity = world.CreateEntity().Attach(new IntComponent { x = 9 }).Entity;
        //     world.CreateEntity().Attach(new IntComponent { x = 11 });

        //     var view = world.CreateView().Select<IntComponent>();

        //     Assert.Throws<InvalidOperationException>(() => {
        //         foreach (var row in view) {
        //             world.Destroy(entity);
        //         }
        //     });
        // }
    }
}