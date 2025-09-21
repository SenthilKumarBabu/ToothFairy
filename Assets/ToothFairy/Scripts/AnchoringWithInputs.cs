﻿/*
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

namespace General
{
    [ExecuteAlways]
    public class AnchoringWithInputs : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("uGUI/Anchors to Corners &[")]
        static void AnchorsToCorners()
        {

            RectTransform t = Selection.activeTransform as RectTransform;
            RectTransform pt = Selection.activeTransform.parent as RectTransform;

            if (t == null || pt == null) return;

            Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                t.anchorMin.y + t.offsetMin.y / pt.rect.height);
            Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                t.anchorMax.y + t.offsetMax.y / pt.rect.height);

            t.anchorMin = newAnchorsMin;
            t.anchorMax = newAnchorsMax;
            t.offsetMin = t.offsetMax = new Vector2(0, 0);

        }
#endif
    }
}
*/

using UnityEditor;
using UnityEngine;

public class AnchoringWithInputs : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("uGUI/Anchors to Corners %[")] // %[ = Ctrl/Cmd + [
    static void AnchorsToCorners()
    {
        // Loop through all selected objects
        foreach (var obj in Selection.transforms)
        {
            RectTransform t = obj as RectTransform;
            RectTransform pt = obj.parent as RectTransform;

            // Skip if this isn't a valid UI element
            if (t == null || pt == null)
                continue;

            // Record for undo
            Undo.RecordObject(t, "Anchors to Corners");

            Vector2 newAnchorsMin = new Vector2(
                t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                t.anchorMin.y + t.offsetMin.y / pt.rect.height
            );

            Vector2 newAnchorsMax = new Vector2(
                t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                t.anchorMax.y + t.offsetMax.y / pt.rect.height
            );

            t.anchorMin = newAnchorsMin;
            t.anchorMax = newAnchorsMax;
            t.offsetMin = t.offsetMax = Vector2.zero;
        }
    }
#endif
}
