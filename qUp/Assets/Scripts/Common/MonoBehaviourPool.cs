using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common {
    public class MonoBehaviourPool<T> where T : MonoBehaviour, IActivable {

        private readonly GameObject prefab;

        private readonly Transform parent;

        private readonly List<T> pool = new List<T>();

        private readonly List<T> usedPool = new List<T>();

        private readonly Func<GameObject, T> monoBehaviourGetter;

        /// <summary>
        /// Pool for gameObjects.
        /// </summary>
        /// <param name="prefab">Prefab from which new gameObjects will be created</param>
        /// <param name="parent">GameObject which will be parent of newly instantiated objects</param>
        /// <param name="monoBehaviourGetter">If null it will use gameObject => gameObject.GetComponent()</param>
        public MonoBehaviourPool(GameObject prefab, Transform parent, Func<GameObject, T> monoBehaviourGetter = null) {
            this.prefab = prefab;
            this.parent = parent;
            this.monoBehaviourGetter = monoBehaviourGetter == null
                ? gameObject => gameObject.GetComponent<T>()
                : this.monoBehaviourGetter;
        }

        /// <summary>
        /// Take a single gameObject from a pool or create a new one if pool is empty.
        /// </summary>
        /// <returns></returns>
        public T TakeObject() {
            var monoBehaviour = pool.FirstOrDefault();
            if (monoBehaviour == default) {
                monoBehaviour = Object.Instantiate(prefab, parent).GetComponent<T>();
            } else {
                pool.Remove(monoBehaviour);
            }

            usedPool.Add(monoBehaviour);
            monoBehaviour.SetActive(true);
            return monoBehaviour;
        }

        /// <summary>
        /// Return a gameObject to pool. Deactivates returned gameObject.
        /// </summary>
        /// <param name="gameObject"></param>
        public void ReturnObject(T gameObject) {
            gameObject.SetActive(false);
            usedPool.Remove(gameObject);
            pool.Add(gameObject);
        }

        /// <summary>
        /// Take a gameObject from a pool or create a new one if pool is empty.
        /// </summary>
        /// <param name="count">Amount of gameObjects</param>
        /// <returns></returns>
        public List<T> TakeObjects(int count) {
            var gameObjects = new List<T>();
            for (var i = 0; i < count; i++) {
                gameObjects.Add(TakeObject());
            }

            return gameObjects;
        }

        /// <summary>
        /// Provides set amount of gameObjects taking active ones first, then from pool and in the end creating new
        /// ones if necessary. 
        /// </summary>
        /// <param name="count">Amount of gameObjects required</param>
        /// <returns></returns>
        public List<T> RetakeObjects(int count) {
            var gameObjects = new List<T>();
            if (usedPool.Count > count) {
                var overflow = usedPool.GetRange(0, usedPool.Count - count);
                ReturnObjects(overflow);
            } else {
                gameObjects = TakeObjects(count - usedPool.Count);
            }

            gameObjects.AddRange(usedPool);
            return gameObjects;
        }

        /// <summary>
        /// Return multiple gameObjects to pool.
        /// </summary>
        /// <param name="gameObjects">GameObjects to return</param>
        public void ReturnObjects(IEnumerable<T> gameObjects) {
            foreach (var gameObject in gameObjects) {
                ReturnObject(gameObject);
            }
        }

        /// <summary>
        /// Returns all used (active) gameObjects
        /// </summary>
        public void ReturnAll() {
            ReturnObjects(usedPool.ToList());
        }
    }
}
