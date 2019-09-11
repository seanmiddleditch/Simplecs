using NUnit.Framework;
using Simplecs;
using System.Linq;

namespace SimplecsTests {
    struct Comp1 {
        public string name;
    }

    struct Comp2 {
        public int x;
    }

    public class WorldTest {
        [Test]
        public void Create() {
            var world = new World();
            var entity = world.Create()
                .Attach(new Comp1{name="Bob"})
                .Attach(new Comp2{x=7})
                .Entity;

            world.Create()
                .Attach(new Comp1{name="Susan"})
                .Attach(new Comp2{x=90});

            var view1 = new View<Comp1>(world);
            var view2 = new View<Comp2>(world);
            var viewBoth = new View<Comp1, Comp2>(world);

            Assert.IsTrue(view1.Any());
            Assert.IsTrue(view2.Any());
            Assert.IsTrue(viewBoth.Any());

            Assert.AreEqual(expected:(entity, new Comp1{name="Bob"}), actual:view1.FirstOrDefault());
            Assert.AreEqual(expected:(entity, new Comp2{x=7}), actual:view2.FirstOrDefault());
            Assert.AreEqual(expected:(entity, new Comp1{name="Bob"}, new Comp2{x=7}), actual:viewBoth.FirstOrDefault());
        }
    }
}