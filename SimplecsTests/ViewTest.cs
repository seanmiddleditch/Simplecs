using NUnit.Framework;
using Simplecs;
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
    }
}