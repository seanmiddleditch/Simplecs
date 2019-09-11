using NUnit.Framework;
using Simplecs;

namespace SimplecsTests {
    public class EntityAllocatorTest {
        [Test]
        public void AllocateAndFreeList() {
            var allocator = new EntityAllocator();

            Assert.AreEqual(expected:1u, actual:allocator.Allocate());
            Assert.AreEqual(expected:2u, actual:allocator.Allocate());
            Assert.AreEqual(expected:3u, actual:allocator.Allocate());

            allocator.Deallocate(2u);
            allocator.Deallocate(3u);
            allocator.Deallocate(1u);

            Assert.AreEqual(expected:1u, actual:allocator.Allocate());
            Assert.AreEqual(expected:3u, actual:allocator.Allocate());
            Assert.AreEqual(expected:2u, actual:allocator.Allocate());
            Assert.AreEqual(expected:4u, actual:allocator.Allocate());
        }
    }
}
