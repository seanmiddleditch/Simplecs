using System.Collections;
using System.Collections.Generic;

namespace Simplecs {
    public class View<T> : IEnumerable<(Entity, T)> where T : struct {
        private ComponentTable<T> _table;

        public View(World world) => _table = world.Components<T>();

        public IEnumerator<(Entity, T)> GetEnumerator() {
            foreach ((uint key, T data) in _table) {
                yield return (new Entity{key=key}, data);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }

    public class View<T1, T2> : IEnumerable<(Entity, T1, T2)> where T1 : struct where T2 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;

        public View(World world) => (_table1, _table2) = (world.Components<T1>(), world.Components<T2>());

        public IEnumerator<(Entity, T1, T2)> GetEnumerator() {
            foreach ((uint key, T1 data1) in _table1) {
                if (_table2.TryGet(key, out T2 data2)) {
                    yield return (new Entity{key=key}, data1, data2);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}