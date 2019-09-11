using NUnit.Framework;
using Simplecs;
using System.Linq;

namespace SimplecsTests {
    public class ComponentTableTest {
        [Test]
        public void Set() {
            var table = new ComponentTable<Comp1>();

            table.Set(7u, new Comp1{name="Bob"});
            table.Set(90u, new Comp1{name="Susan"});
            table.Set(2u, new Comp1{name="Frank"});

            Assert.AreEqual(expected:3, actual:table.Count());
        }

        [Test]
        public void SetReplace() {
            var table = new ComponentTable<Comp1>();

            table.Set(7u, new Comp1{name="Bob"});
            table.Set(7u, new Comp1{name="Susan"});

            Assert.AreEqual(expected:1, table.Count());
            Assert.AreEqual(expected:(7, new Comp1{name="Susan"}), actual:table.FirstOrDefault());
        }

        [Test]
        public void Cycle() {
            var table = new ComponentTable<Comp1>();

            table.Set(7u, new Comp1{name="Bob"});
            table.Set(2u, new Comp1{name="Frank"});
            Assert.AreEqual(expected:2, actual:table.Count());
            var (key, data) = table.FirstOrDefault();
            Assert.AreEqual(expected:7u, actual:key);
            Assert.AreEqual(expected:"Bob", actual:data.name);

            Assert.IsTrue(table.Remove(7u));
            Assert.IsFalse(table.Remove(7u));
            Assert.AreEqual(expected:1, actual:table.Count());

            (key, data) = table.FirstOrDefault();
            Assert.AreEqual(expected:2u, actual:key);
            Assert.AreEqual(expected:"Frank", actual:data.name);

            table.Set(90u, new Comp1{name="Susan"});
            Assert.AreEqual(expected:2, actual:table.Count());

            Assert.IsTrue(table.Remove(2u));
            Assert.AreEqual(expected:1, actual:table.Count());

            (key, data) = table.FirstOrDefault();
            Assert.AreEqual(expected:90u, actual:key);
            Assert.AreEqual(expected:"Susan", actual:data.name);
        }
    }
}