using System.Collections.Generic;
using UnityEngine;

namespace Assets.Pathfinding._Move
{
    public class PrefabInstancePool
    {
        private readonly Transform container;
        private readonly Transform prefab;
        private List<Transform> instances = new List<Transform>();

        private int currentFrameCounter = 0;

        public PrefabInstancePool(Transform container, Transform prefab)
        {
            this.container = container;
            this.prefab = prefab;
        }

        public void Clear()
        {
            currentFrameCounter = 0;
        }

        public Transform Take()
        {
            while(currentFrameCounter >= instances.Count)
            {
                var inst = Object.Instantiate(prefab, container);
                instances.Add(inst);
            }

            var ret = instances[currentFrameCounter];
            ret.gameObject.SetActive(true);

            currentFrameCounter++;
            return ret;

        }

        public void DoneTaking()
        {
            for (int i = currentFrameCounter; i < instances.Count; i++)
            {
                instances[i].gameObject.SetActive(false);
            }

            currentFrameCounter = instances.Count;
        }

    }
}