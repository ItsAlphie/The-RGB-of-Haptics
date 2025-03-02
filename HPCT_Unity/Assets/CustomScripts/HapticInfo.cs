using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HapticInfo))]
public class DropdownEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HapticInfo script = (HapticInfo)target;

        GUIContent arrayLabel = new GUIContent("Presets");
        int newArrayIdx = EditorGUILayout.Popup(arrayLabel, script.arrayIdx, script.Presets);

        if (newArrayIdx != script.arrayIdx)
        {
            script.arrayIdx = newArrayIdx;
            UpdatePresetValues(script);
        }
    }

    private void UpdatePresetValues(HapticInfo script)
    {
        switch (script.arrayIdx)
        {
            case 0:
                // Custom
                break;
            case 1:
                // Wood
                script.Roughness = 0.95f;
                script.BumpsSize = 0.2f;
                script.Temperature = 0f;
                break;
            case 2:
                // Steel
                script.Roughness = 0.48f;
                script.BumpsSize = 0f;
                script.Temperature = -0.76f;
                break;
        }
    }
}

public class HapticInfo : MonoBehaviour
{
    [HideInInspector]
    public int arrayIdx = 0;
    [HideInInspector]
    public string[] Presets = new string[] { "Custom", "Wood", "Steel" };

    [Range(0.0f, 1.0f)] public float Roughness = 0f;
    [Range(0.0f, 1.0f)] public float BumpsSize = 0f;
    [Range(-1.0f, 1.0f)] public float Temperature = 0f;
}
