using System;
using UnityEngine;

namespace Assets.Modules.MHGameWork.Reusable.UnityAdditions
{
    /// <summary>
    /// This ManualSingleton is used for providing global access to a scene unique singleton.
    /// The idea of these singletons is that they are preconfigured by the user in the scene, preferably by dropping in a prefab with the singleton script.
    /// Example use case is a Script that has field linked to other prefabs, like a PrefabFactory or ResourceFactory
    ///
    /// Does not automatically create an instance, needs one created by the user in the scene.
    /// Does not automatically persist across scenes
    ///
    /// Basically this is not a singleton but more like a service locator for a single instance.
    /// 
    /// This ManualSingleton differes from the other singleton in that it DOES NOT AUTOMATICALLY CREATE AN INSTANCE
    /// 
    /// </summary>
    public class ManualSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool hasChecked;

        private static T _instance;

        private static object _lock = new object();

        public static T Instance => getInstanceInternal();

        /// <summary>
        /// Make this work when scenes unload
        /// </summary>
        public void OnDisable()
        {
            lock (_lock)
            {
                _instance = null;
            }
        }

        private static T getInstanceInternal()
        {
            findInstance();

            if (_instance == null)
            {
                throw new Exception(
                    $"You should manually provide an instance of {typeof(T).Name} in the scene, preferably from a prefab");
            }

            return _instance;
        }

        private static T findInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    var get = (T)FindObjectOfType(typeof(T));


                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError($"There should only be one instance of the Manual Singleton: {typeof(T).Name}");
                    }

                    _instance = get;

                    hasChecked = true;
                    return get;
                }

                return _instance;
            }
        }

        public static bool HasInstance
        {
            get
            {
                lock (_lock)
                {
                    if (!hasChecked) findInstance();
                    return _instance != null;
                }

            }
        }

        /// <summary>
        /// Creates the manual singleton from code if doesnt exist
        /// </summary>
        public static void EnsureCreated()
        {
            if (!HasInstance) CreateDefault();

        }
        public static void CreateDefault()
        {
            var g = new GameObject($"{typeof(T).Name} (autocreated)");
            g.AddComponent<T>();
        }
    }
}