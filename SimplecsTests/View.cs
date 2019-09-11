using NUnit.Framework;
using Simplecs;
using System.Linq;

namespace SimplecsTests {
    public class ViewTest {
        [Test]
        public void EachMutable() {
            var world = new World();
            world.Create().Attach(new Comp2{x = 1});
            world.Create().Attach(new Comp2{x = 2});
            world.Create().Attach(new Comp2{x = 3});

            var view = new View<Comp2>(world);
            view.Each((Entity _, ref Comp2 comp) => comp.x *= comp.x);

            Assert.AreEqual(expected:1, actual:view.ElementAt(0).Item2.x);
            Assert.AreEqual(expected:4, actual:view.ElementAt(1).Item2.x);
            Assert.AreEqual(expected:9, actual:view.ElementAt(2).Item2.x);
        }
    }
}