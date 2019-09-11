using NUnit.Framework;
using Simplecs;

namespace SimplecsTests {
    public class EntityAllocatorTest {
        [Test]
        public void AllocateAndFreeList() {
            var allocator = new EntityAllocator(freeMinimum:0);

            Assert.AreEqual(expected:EntityUtil.MakeKey(1), actual:allocator.Allocate());
            Assert.AreEqual(expected:EntityUtil.MakeKey(2), actual:allocator.Allocate());
            Assert.AreEqual(expected:EntityUtil.MakeKey(3), actual:allocator.Allocate());

            allocator.Deallocate(EntityUtil.MakeKey(2));
            allocator.Deallocate(EntityUtil.MakeKey(3));
            allocator.Deallocate(EntityUtil.MakeKey(1));

            // Generation will be bumped for the recycled indices.
            //
            Assert.AreEqual(expected:EntityUtil.MakeKey(2, generation:1), actual:allocator.Allocate());
            Assert.AreEqual(expected:EntityUtil.MakeKey(3, generation:1), actual:allocator.Allocate());
            Assert.AreEqual(expected:EntityUtil.MakeKey(1, generation:1), actual:allocator.Allocate());

            // But not for new indices.
            //
            Assert.AreEqual(expected:EntityUtil.MakeKey(4), actual:allocator.Allocate());
        }
    }
}
