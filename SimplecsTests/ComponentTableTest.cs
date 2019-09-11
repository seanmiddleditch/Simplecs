using NUnit.Framework;
using Simplecs;
using System.Linq;

namespace SimplecsTests {
    public class ComponentTableTest {
        static readonly Entity key1 = EntityUtil.MakeKey(7);
        static readonly Entity key2 = EntityUtil.MakeKey(90, generation:4);
        static readonly Entity key3 = EntityUtil.MakeKey(2, generation:2);

        [Test]
        public void Set() {
            var table = new ComponentTable<Comp1>();

            table.Add(key1, new Comp1{name="Bob"});
            table.Add(key2, new Comp1{name="Susan"});
            table.Add(key3, new Comp1{name="Frank"});

            Assert.AreEqual(expected:3, actual:table.Count());
        }

        [Test]
        public void SetReplace() {
            var table = new ComponentTable<Comp1>();

            table.Add(key1, new Comp1{name="Bob"});
            table.Add(key1, new Comp1{name="Susan"});

            Assert.AreEqual(expected:1, table.Count());
            Assert.AreEqual(expected:(key1, new Comp1{name="Susan"}), actual:table.FirstOrDefault());
        }

        [Test]
        public void Cycle() {
            var table = new ComponentTable<Comp1>();

            table.Add(key1, new Comp1{name="Bob"});
            table.Add(key3, new Comp1{name="Frank"});
            Assert.AreEqual(expected:2, actual:table.Count());
            var (key, data) = table.FirstOrDefault();
            Assert.AreEqual(expected:key1, actual:key);
            Assert.AreEqual(expected:"Bob", actual:data.name);

            Assert.IsTrue(table.Remove(key1));
            Assert.IsFalse(table.Remove(key1));
            Assert.AreEqual(expected:1, actual:table.Count());

            (key, data) = table.FirstOrDefault();
            Assert.AreEqual(expected:key3, actual:key);
            Assert.AreEqual(expected:"Frank", actual:data.name);

            table.Add(key2, new Comp1{name="Susan"});
            Assert.AreEqual(expected:2, actual:table.Count());

            Assert.IsTrue(table.Remove(key3));
            Assert.AreEqual(expected:1, actual:table.Count());

            (key, data) = table.FirstOrDefault();
            Assert.AreEqual(expected:key2, actual:key);
            Assert.AreEqual(expected:"Susan", actual:data.name);
        }
    }
}