#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Assets.Modules.MHGameWork.Reusable.UnityAdditions
{
    /// <summary>
    /// Various utilities for interacting with the unity editor.
    /// Saveguarded with precompiler directives so they are save to use in playmode scripts
    /// </summary>
    public class EditorUtilities
    {
        public static void SelectGameObject(GameObject activeGameObject,bool showInSceneView = true, bool showInHierarchy = true)
        {
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(activeGameObject);
            if (activeGameObject != null)
                Selection.activeGameObject = activeGameObject;
            SceneView.lastActiveSceneView.FrameSelected();


#endif
        }

        public static void SetScriptableObjectDirty(ScriptableObject obj)
        {
#if UNITY_EDITOR

            EditorUtility.SetDirty(obj);
#endif
        }
    }
}