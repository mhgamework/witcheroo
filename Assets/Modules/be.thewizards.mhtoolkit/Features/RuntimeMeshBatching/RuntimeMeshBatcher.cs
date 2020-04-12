using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Reusable;
using Assets.UnityAdditions;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace Modules.Utilities.RuntimeMeshBatching
{
    /// <summary>
    /// Responsible for combining multiple gameobject's meshes into batched meshes
    /// Existing objects get disabled
    /// Chunks are currently not implemented
    /// 
    /// </summary>
    public class RuntimeMeshBatcher
    {
        private readonly float chunkSize;
        private readonly Transform batchesContainer;
        private Dictionary<MeshRenderer, BatchScript> batchLookup = new Dictionary<MeshRenderer, BatchScript>();

        public RuntimeMeshBatcher(float chunkSize, Transform batchesContainer)
        {
            this.chunkSize = chunkSize;
            this.batchesContainer = batchesContainer;
        }

        private List<GameObject> objects = new List<GameObject>();
        private List<GameObject> changedObjects = new List<GameObject>();

        public void RegisterChildren(Transform parent)
        {
            objects.AddRange(parent.GetChildren<Transform>().Select(t => t.gameObject));
        }

        public void RegisterObjects(IEnumerable<GameObject> newObjects)
        {
            objects.AddRange(newObjects);
        }

        public void MarkUnbatch(GameObject obj)
        {
            changedObjects.Add(obj);
        }

        public IEnumerable<YieldInstruction> RebatchChanges()
        {
            var affectedBatches = new HashSet<BatchScript>();
            var newRenderers = new HashSet<MeshRenderer>();
            var allRenderers = getAllMeshRenderers(changedObjects).Distinct();

            foreach (var changedRenderer in allRenderers)
            {
                if (batchLookup.TryGetValue(changedRenderer, out var batch))
                {
                    batch.BatchedRenderers.Remove(changedRenderer);
                    changedRenderer.enabled = true;
                    Object.Destroy(changedRenderer.GetComponent<BatchedMeshScript>());

                    affectedBatches.Add(batch);
                }
                else
                {
                    newRenderers.Add(changedRenderer);
                }
            }

            // Dont do anything with new renderers, but rebatch the old ones
            yield return null;
            foreach (var f in affectedBatches)
            {
                f.SetMesh(BatchGroup.combineMeshRenderers(f.BatchedRenderers));
            }
        }

        public IEnumerator BatchCoroutine()
        {
            foreach (var f in GetBatchSteps())
            {
                f();
                yield return null;
            }
        }

        /// <summary>
        /// Splitting this into steps so it can be put in the loading screen
        /// Must be run in sequence otherwise kaboom
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Action> GetBatchSteps()
        {
            var allRenderers = getAllMeshRenderers(objects).Distinct();

            var groups = new Dictionary<string, BatchGroup>();

            /* yield return () =>
             {*/
            Profiler.BeginSample("RuntimeMeshBatcher.AddRenderers");

            foreach (var mesh in allRenderers)
            {
                var key = mesh.sharedMaterial.GetInstanceID() + "_" + mesh.gameObject.layer;

                var group = groups.GetOrCreate(key,
                    m => new BatchGroup(mesh.sharedMaterial, mesh.gameObject.layer, this));
                group.AddRenderer(mesh);
                //                yield return null;
            }

            
            Profiler.EndSample();
            //};


            foreach (var g in groups.Values)
            {
                yield return () =>
                {
                    Profiler.BeginSample("RuntimeMeshBatcher.BuildMeshes");
                    g.CreateBatches();
                    g.BuildBatchedMeshes();
                    Profiler.EndSample();
                };
            }
        }

		private IEnumerable<MeshRenderer> getAllMeshRenderers(List<GameObject> gameObjects)
		{
			foreach (var g in gameObjects)
			{
				foreach (var meshRenderer in getAllMeshRenderersRecurse(g)) yield return meshRenderer;
			}
		}

		private static IEnumerable<MeshRenderer> getAllMeshRenderersRecurse(GameObject g)
		{
			var options = g.GetComponent<MeshBatchingOptionsScript>();
			if (options != null && options.DontBatch) yield break;

			foreach (var r in g.transform.GetChildren<Transform>())
			{
				foreach (var ret in getAllMeshRenderersRecurse(r.gameObject)) yield return ret;
			}

			foreach (var c in g.GetComponents<MeshRenderer>()) yield return c;
		}

		private class BatchGroup
        {
            public string Key;
            public Material sharedMaterial;
            public int layer;
            private readonly RuntimeMeshBatcher batcher;
            private List<MeshRenderer> batchedRenderers = new List<MeshRenderer>();
            private List<BatchScript> batches = new List<BatchScript>();

            public BatchGroup(Material sharedMaterial, int layer, RuntimeMeshBatcher batcher)
            {
                this.sharedMaterial = sharedMaterial;
                this.layer = layer;
                this.batcher = batcher;
            }

            public void AddRenderer(MeshRenderer mesh)
            {
                batchedRenderers.Add(mesh);
            }

            public void CreateBatches()
            {
                BatchScript batchScript = null;
                var meshBuffer = new List<CombineInstance>();
                var vertexCount = 0;
                foreach (var r in batchedRenderers)
                {
                    var meshFilter = r.GetComponent<MeshFilter>();
                    if (meshFilter == null) continue; // do nothing

                    var newMesh = meshFilter.sharedMesh;
                    if (newMesh == null) continue; // do nothing
                    if (newMesh.subMeshCount > 1)
                    {
                        Debug.LogError("Cannot batch mesh with submeshes, not implemented!", r);
                        continue;
                    }

                    // do twice max
                    for (int i = 0; i < 2; i++)
                    {
                        if (batchScript == null)
                        {
                            batchScript = batcher.CreateBatchScript(sharedMaterial, layer);

                            meshBuffer.Clear();
                            vertexCount = 0;
                        }

                        var shouldAddToExistingBatch = (vertexCount == 0 || vertexCount + newMesh.vertexCount <= 65000);

                        if (!shouldAddToExistingBatch)
                        {
                            // full, so build first, then repeat
                            batches.Add(batchScript);
                            batchScript = null;
                            continue;
                        }

                        //TODO: submeshes
                        batchScript.AddBatchedRenderer(r);
                        batcher.batchLookup.Add(r, batchScript);
                        vertexCount += newMesh.vertexCount;

                        break;
                    }
                }

                if (vertexCount > 0)
                {
                    batches.Add(batchScript);
                    batchScript = null;
                }
            }

            public void BuildBatchedMeshes()
            {
                foreach (var b in batches)
                {
                    b.SetMesh(combineMeshRenderers(b.BatchedRenderers));
                    foreach (var r in b.BatchedRenderers)
                    {
                        if (r.GetComponent<BatchedMeshScript>() != null)
                            throw new Exception("Object is already batched by some batch");
                        r.gameObject.AddComponent<BatchedMeshScript>();
                        r.enabled = false;
                    }
                }
            }

            public static Mesh combineMeshRenderers(List<MeshRenderer> renderers)
            {
                var m = new Mesh();

                m.CombineMeshes(renderers.Select(r => new CombineInstance()
                {
                    mesh = r.GetComponent<MeshFilter>().sharedMesh,
                    transform = r.transform.localToWorldMatrix
                }).ToArray(), true);

                return m;
            }
        }

        private BatchScript CreateBatchScript(Material sharedMaterial, int layer)
        {
            var ret = BatchScript.Create(sharedMaterial, layer);
            ret.transform.SetParent(batchesContainer);
            ret.gameObject.name = $"Batch [{sharedMaterial.name}] Layer [{layer}";
            return ret;
        }

        private class BatchScript : MonoBehaviour
        {
            private Material sharedMaterial;
            private int layer;
            private MeshFilter filter;
            private MeshRenderer meshRenderer;
            public List<MeshRenderer> BatchedRenderers = new List<MeshRenderer>();

            private void init(Material sharedMaterial, int layer)
            {
                this.sharedMaterial = sharedMaterial;
                this.layer = layer;
                filter = gameObject.AddComponent<MeshFilter>();
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = sharedMaterial;
            }

            public static BatchScript Create(Material sharedMaterial, int layer)
            {
                var r = new GameObject("Batch");
                r.layer = layer;
                var ret = r.AddComponent<BatchScript>();
                ret.init(sharedMaterial, layer);
                return ret;
            }

            public void SetMesh(Mesh buildMesh)
            {
                filter.mesh = buildMesh;
            }

            public void AddBatchedRenderer(MeshRenderer renderer)
            {
                BatchedRenderers.Add(renderer);
            }
        }
    }
}