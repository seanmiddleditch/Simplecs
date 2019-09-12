using NUnit.Framework;
using Simplecs;
using System.Linq;

namespace SimplecsTests {
    public class WorldTest {
        [Test]
        public void Create() {
            var world = new World();
            var entity = world.CreateEntity()
                .Attach(new NameComponent { name = "Bob" })
                .Attach(new IntComponent { x = 7 })
                .Entity;

            world.CreateEntity()
                .Attach(new NameComponent { name = "Susan" })
                .Attach(new IntComponent { x = 90 });

            var names = world.GetTable<NameComponent>();
            var ints = world.GetTable<IntComponent>();

            Assert.AreEqual(expected: 2, actual: names.Count);
            Assert.AreEqual(expected: 2, actual: ints.Count);
        }

        [Test]
        public void Destroy() {
            var world = new World();
            var entity = world.CreateEntity()
                .Attach(new NameComponent { name = "Bob" })
                .Entity;

            world.CreateEntity()
                .Attach(new NameComponent { name = "Susan" })
                .Attach(new IntComponent { x = 90 });

            var names = world.GetTable<NameComponent>();
            var ints = world.GetTable<IntComponent>();

            Assert.AreEqual(expected: 2, actual: names.Count);
            Assert.AreEqual(expected: 1, actual: ints.Count);

            world.Destroy(entity);

            Assert.AreEqual(expected: 1, actual: names.Count);
            Assert.AreEqual(expected: 1, actual: ints.Count);
        }
    }
}