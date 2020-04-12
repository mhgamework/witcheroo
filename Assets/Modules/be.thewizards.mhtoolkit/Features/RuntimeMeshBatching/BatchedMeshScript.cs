using EasyButtons;
using UnityEngine;

namespace Modules.Utilities.RuntimeMeshBatching
{
    /// <summary>
    /// Marker script to mark a batched mesh
    /// </summary>
    public class BatchedMeshScript : MonoBehaviour
    {
//        /// <summary>
//        /// For manual testing purposes only, not meant for use in production
//        /// </summary>
//        [Button]
//        public void Unbatch()
//        {
//            var runtimeMeshBatcher = FindObjectOfType<RuntimeMeshBatcherTestScript>().batcher;
//            runtimeMeshBatcher.MarkUnbatch(gameObject);
//            foreach (var f in runtimeMeshBatcher.RebatchChanges()) ; // Note; in reality, do this at the end of frame, not for each unbatch
//        }
    }
}