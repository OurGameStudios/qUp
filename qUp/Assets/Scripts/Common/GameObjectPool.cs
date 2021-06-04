using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common {
    public class GameObjectPool {
        
        private readonly GameObject prefab;

        private readonly Transform parent;

        private readonly List<GameObject> pool = new List<GameObject>();

        private readonly List<GameObject> usedPool = new List<GameObject>();

        /// <summary>
        /// Pool for gameObjects.
        /// </summary>
        /// <param name="prefab">Prefab from which new gameObjects will be created</param>
        /// <param name="parent"></param>
        public GameObjectPool(GameObject prefab, Transform parent) {
            this.prefab = prefab;
            this.parent = parent;
        }

        /// <summary>
        /// Take a single gameObject from a pool or create a new one if pool is empty.
        /// </summary>
        /// <returns></returns>
        public GameObject TakeGameObject() {
            var gameObject = pool.FirstOrDefault();
            if (gameObject == default) {
                gameObject = Object.Instantiate(prefab, parent);
            } else {
                pool.Remove(gameObject);
            }
            usedPool.Add(gameObject);
            gameObject.SetActive(true);
            return gameObject;
        }

        /// <summary>
        /// Return a gameObject to pool. Deactivates returned gameObject.
        /// </summary>
        /// <param name="gameObject"></param>
        public void ReturnGameObject(GameObject gameObject) {
            gameObject.SetActive(false);
            usedPool.Remove(gameObject);
            pool.Add(gameObject);
        }

        /// <summary>
        /// Take a gameObject from a pool or create a new one if pool is empty.
        /// </summary>
        /// <param name="count">Amount of gameObjects</param>
        /// <returns></returns>
        public List<GameObject> TakeGameObjects(int count) {
            var gameObjects = new List<GameObject>();
            for (var i = 0; i < count; i++) {
                gameObjects.Add(TakeGameObject());
            }
            return gameObjects;
        }

        /// <summary>
        /// Provides set amount of gameObjects taking active ones first, then from pool and in the end creating new
        /// ones if necessary. 
        /// </summary>
        /// <param name="count">Amount of gameobjects required</param>
        /// <returns></returns>
        public List<GameObject> RetakeGameObjects(int count) {
            var gameObjects = new List<GameObject>();
            if (usedPool.Count > count) {
                var overflow = usedPool.GetRange(0, usedPool.Count - count);
                ReturnGameObjects(overflow);
            } else {
                gameObjects = TakeGameObjects(count - usedPool.Count);
            }
            gameObjects.AddRange(usedPool);
            return gameObjects;
        }

        /// <summary>
        /// Return multiple gameObjects to pool.
        /// </summary>
        /// <param name="gameObjects">GameObjects to return</param>
        public void ReturnGameObjects(IEnumerable<GameObject> gameObjects) {
            foreach (var gameObject in gameObjects) {
                ReturnGameObject(gameObject);
            }
        }
    }
}
