#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Assets.UnityAdditions
{
    public class AssetDatabaseUtility
    {
        public static void CreateFolderIfDoesntExist(string parentFolder, string newFolderName)
        {
#if UNITY_EDITOR
            if (!(AssetDatabase.IsValidFolder($"{parentFolder}/{newFolderName}")))
                AssetDatabase.CreateFolder(parentFolder, newFolderName);
#endif
        }
    }
}

