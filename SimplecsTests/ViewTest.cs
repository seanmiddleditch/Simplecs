using NUnit.Framework;
using Simplecs;
using System.Linq;

namespace SimplecsTests {
    public class ViewTest {
        [Test]
        public void EachMutable() {
            var world = new World();
            world.Create().Attach(new IntComponent { x = 1 });
            world.Create().Attach(new IntComponent { x = 2 });
            world.Create().Attach(new IntComponent { x = 3 });

            var view = new View<IntComponent>(world);
            view.Each((Entity _, ref IntComponent comp) => comp.x *= comp.x);

            Assert.AreEqual(expected: 1, actual: view.ElementAt(0).Item2.x);
            Assert.AreEqual(expected: 4, actual: view.ElementAt(1).Item2.x);
            Assert.AreEqual(expected: 9, actual: view.ElementAt(2).Item2.x);
        }

        [Test]
        public void Require() {
            var world = new World();
            world.Create().Attach(new IntComponent { x = 1 });
            world.Create().Attach(new NameComponent { name = "Bob" }).Attach(new IntComponent { x = 2 });
            world.Create().Attach(new IntComponent { x = 3 });
            world.Create().Attach(new NameComponent { name = "Susan" }).Attach(new IntComponent { x = 4 });

            var view = new View<IntComponent>(world).Require<NameComponent>();

            Assert.AreEqual(expected: 2, actual: view.ElementAt(0).Item2.x);
            Assert.AreEqual(expected: 4, actual: view.ElementAt(1).Item2.x);
        }

        [Test]
        public void Exclude() {
            var world = new World();
            world.Create().Attach(new NameComponent { name = "Bob" }).Attach(new IntComponent { x = 2 });
            world.Create().Attach(new IntComponent { x = 1 });
            world.Create().Attach(new NameComponent { name = "Susan" }).Attach(new IntComponent { x = 4 });
            world.Create().Attach(new IntComponent { x = 3 });

            var view = new View<IntComponent>(world).Exclude<NameComponent>();

            Assert.AreEqual(expected: 1, actual: view.ElementAt(0).Item2.x);
            Assert.AreEqual(expected: 3, actual: view.ElementAt(1).Item2.x);
        }

        [Test]
        public void Match() {
            var world = new World();
            var entity = world.Create()
                .Attach(new NameComponent { name = "Bob" })
                .Attach(new IntComponent { x = 7 })
                .Entity;

            world.Create()
                .Attach(new NameComponent { name = "Susan" })
                .Attach(new IntComponent { x = 90 });

            var nameView = new View<NameComponent>(world);
            var intView = new View<IntComponent>(world);
            var bothView = new View<NameComponent, IntComponent>(world);

            Assert.IsTrue(nameView.Any());
            Assert.IsTrue(intView.Any());
            Assert.IsTrue(bothView.Any());

            Assert.AreEqual(expected: (entity, new NameComponent { name = "Bob" }), actual: nameView.FirstOrDefault());
            Assert.AreEqual(expected: (entity, new IntComponent { x = 7 }), actual: intView.FirstOrDefault());
            Assert.AreEqual(expected: (entity, new NameComponent { name = "Bob" }, new IntComponent { x = 7 }), actual: bothView.FirstOrDefault());
        }
    }
}