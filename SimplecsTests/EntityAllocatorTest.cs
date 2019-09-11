using NUnit.Framework;
using Simplecs;

namespace SimplecsTests {
    public class EntityAllocatorTest {
        [Test]
        public void AllocateAndFreeList() {
            var allocator = new EntityAllocator(freeMinimum:0);

            Assert.AreEqual(expected:1u, actual:allocator.Allocate());
            Assert.AreEqual(expected:2u, actual:allocator.Allocate());
            Assert.AreEqual(expected:3u, actual:allocator.Allocate());

            allocator.Deallocate(2u);
            allocator.Deallocate(3u);
            allocator.Deallocate(1u);

            // Generation will be bumped for the recycled indices.
            //
            Assert.AreEqual(expected:EntityAllocator.MakeKey(2u, 1), actual:allocator.Allocate());
            Assert.AreEqual(expected:EntityAllocator.MakeKey(3u, 1), actual:allocator.Allocate());
            Assert.AreEqual(expected:EntityAllocator.MakeKey(1u, 1), actual:allocator.Allocate());

            // But not for new indices.
            //
            Assert.AreEqual(expected:4u, actual:allocator.Allocate());
        }
    }
}
