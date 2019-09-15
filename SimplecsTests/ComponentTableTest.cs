using NUnit.Framework;
using Simplecs;
using Simplecs.Containers;
using System.Linq;

namespace SimplecsTests {
    public class ComponentTableTest {
        static readonly Entity key1 = EntityUtil.MakeKey(7);
        static readonly Entity key2 = EntityUtil.MakeKey(90, generation: 4);
        static readonly Entity key3 = EntityUtil.MakeKey(2, generation: 2);

        [Test]
        public void Set() {
            var table = new ComponentTable<NameComponent>();

            table.Add(key1, new NameComponent { name = "Bob" });
            table.Add(key2, new NameComponent { name = "Susan" });
            table.Add(key3, new NameComponent { name = "Frank" });

            Assert.AreEqual(expected: 3, actual: table.Count);
        }

        [Test]
        public void SetReplace() {
            var table = new ComponentTable<NameComponent>();

            table.Add(key1, new NameComponent { name = "Bob" });
            table.Add(key1, new NameComponent { name = "Susan" });

            Assert.AreEqual(expected: 1, table.Count);
            Assert.AreEqual(expected: key1, actual: table.EntityAt(0));
            Assert.AreEqual(expected: "Susan", actual: table.TryGet(key1, out var name) ? name.name : null);
        }

        [Test]
        public void Cycle() {
            var table = new ComponentTable<NameComponent>();

            table.Add(key1, new NameComponent { name = "Bob" });
            table.Add(key3, new NameComponent { name = "Frank" });
            Assert.AreEqual(expected: 2, actual: table.Count);
            Assert.AreEqual(expected: key1, actual: table.EntityAt(0));
            Assert.AreEqual(expected: "Bob", actual: table.TryGet(key1, out var name) ? name.name : null);

            Assert.IsTrue(table.Remove(key1));
            Assert.IsFalse(table.Remove(key1));
            Assert.AreEqual(expected: 1, actual: table.Count);

            Assert.AreEqual(expected: key3, actual: table.EntityAt(0));
            Assert.AreEqual(expected: "Frank", actual: table.TryGet(key3, out name) ? name.name : null);

            table.Add(key2, new NameComponent { name = "Susan" });
            Assert.AreEqual(expected: 2, actual: table.Count);

            Assert.IsTrue(table.Remove(key3));
            Assert.AreEqual(expected: 1, actual: table.Count);

            Assert.AreEqual(expected: key2, actual: table.EntityAt(0));
            Assert.AreEqual(expected: "Susan", actual: table.TryGet(key2, out name) ? name.name : null);
        }

        [Test]
        public void Index() {
            var table = new ComponentTable<IntComponent>();

            table.Add(key1, new IntComponent { x = 1 });
            table.Add(key2, new IntComponent { x = 2 });
            table.Add(key3, new IntComponent { x = 3 });

            Assert.AreEqual(expected: 2, actual: table.TryGet(key2, out var intComp) ? intComp.x : 0);
            Assert.AreEqual(expected: 3, actual: table.TryGet(key3, out intComp) ? intComp.x : 0);

            table.Remove(key2);

            table.Add(key2, new IntComponent { x = 4 });

            Assert.AreEqual(expected: 4, actual: table.TryGet(key2, out intComp) ? intComp.x : 0);
            Assert.AreEqual(expected: 3, actual: table.TryGet(key3, out intComp) ? intComp.x : 0);
        }
    }
}