using System.Collections.Generic;
using NUnit.Framework;
using Simplecs;
using Simplecs.Containers;

namespace SimplecsTests {
    public class EntityAllocatorTest {
        [Test]
        public void AllocateAndFreeList() {
            var allocator = new EntityAllocator();

            Assert.AreEqual(expected: EntityUtil.MakeKey(0), actual: allocator.Allocate());
            Assert.AreEqual(expected: EntityUtil.MakeKey(1), actual: allocator.Allocate());
            Assert.AreEqual(expected: EntityUtil.MakeKey(2), actual: allocator.Allocate());

            // Generate enough trash to ensure ids are recycled after we delete them below.
            //
            var trash = new List<Entity>(capacity: EntityAllocator.FreeMinimum);
            for (int counter = 0; counter != EntityAllocator.FreeMinimum; ++counter) {
                trash.Add(allocator.Allocate());
            }

            Assert.IsTrue(allocator.Deallocate(EntityUtil.MakeKey(1)));
            Assert.IsTrue(allocator.Deallocate(EntityUtil.MakeKey(2)));
            Assert.IsFalse(allocator.Deallocate(EntityUtil.MakeKey(EntityAllocator.FreeMinimum + 3)));
            Assert.IsTrue(allocator.Deallocate(EntityUtil.MakeKey(0)));

            // Deallocate all the trash so the free list is full
            //
            foreach (Entity trashEntity in trash) {
                allocator.Deallocate(trashEntity);
            }

            // Generation will be bumped for the recycled indices.
            //
            Assert.AreEqual(expected: EntityUtil.MakeKey(1, generation: 2), actual: allocator.Allocate());
            Assert.AreEqual(expected: EntityUtil.MakeKey(2, generation: 2), actual: allocator.Allocate());
            Assert.AreEqual(expected: EntityUtil.MakeKey(0, generation: 2), actual: allocator.Allocate());

            // Exhaust keys again so we are allocating only new entities
            for (int counter = 0; counter != EntityAllocator.FreeMinimum; ++counter) {
                allocator.Allocate();
            }

            // But not for new indices.
            //
            int totalAllocated = EntityAllocator.FreeMinimum + EntityAllocator.FreeMinimum + 2;
            Assert.AreEqual(expected: EntityUtil.MakeKey(totalAllocated), actual: allocator.Allocate());
        }
    }
}
