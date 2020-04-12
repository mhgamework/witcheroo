#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System;
namespace Assets.Modules.MHGameWork.Reusable.Editor
{
    /// <summary>
    /// Remove empty folders automatically.
    /// Also provides manual button
    /// </summary>
    public class RemoveEmptyFolders : UnityEditor.AssetModificationProcessor
    {
        public const string kRemoveButtonMenuText = "Assets/Remove Empty Folders";
        static readonly StringBuilder s_Log = new StringBuilder();
        static readonly List<DirectoryInfo> s_Results = new List<DirectoryInfo>();


        private static void RemoveEmptyDirectories()
        {
// Get empty directories in Assets directory
            s_Results.Clear();
            var assetsDir = Application.dataPath + Path.DirectorySeparatorChar;
            GetEmptyDirectories(new DirectoryInfo(assetsDir), s_Results);

            // When empty directories has detected, remove the directory.
            if (0 < s_Results.Count)
            {
                s_Log.Length = 0;
                s_Log.AppendFormat("Remove {0} empty directories as following:\n", s_Results.Count);
                foreach (var d in s_Results)
                {
                    s_Log.AppendFormat("- {0}\n", d.FullName.Replace(assetsDir, ""));
                    FileUtil.DeleteFileOrDirectory(d.FullName);
                }

                // UNITY BUG: Debug.Log can not set about more than 15000 characters.
                s_Log.Length = Mathf.Min(s_Log.Length, 15000);
                Debug.Log(s_Log.ToString());
                s_Log.Length = 0;

                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// Removes empty folders.
        /// </summary>
        [MenuItem(kRemoveButtonMenuText)]
        static void OnClickRemoveButtonMenu()
        {
            RemoveEmptyDirectories();
        }


        /// <summary>
        /// Get empty directories.
        /// </summary>
        static bool GetEmptyDirectories(DirectoryInfo dir, List<DirectoryInfo> results)
        {
            bool isEmpty = true;
            try
            {
                isEmpty = dir.GetDirectories().Count(x => !GetEmptyDirectories(x, results)) == 0    // Are sub directories empty?
                        && dir.GetFiles("*.*").All(x => x.Extension == ".meta");    // No file exist?
            }
            catch
            {
            }

            // Store empty directory to results.
            if (isEmpty)
                results.Add(dir);
            return isEmpty;
        }
    }
}
#endif