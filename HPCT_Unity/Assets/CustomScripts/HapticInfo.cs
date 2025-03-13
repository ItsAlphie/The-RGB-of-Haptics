using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Diagnostics;

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
                script.Roughness = 0f;
                script.RoughnessDensity = 0f;
                script.BumpSize = 0f;
                script.BumpDensity = 0f;
                script.Hardness = 0f;
                script.Temperature = 0f;
                break;
            case 1:
                // Wood
                script.Roughness = 0.66f;
                script.RoughnessDensity = 0.80f;
                script.BumpSize = 0.5f;
                script.BumpDensity = 0.20f;
                script.Hardness = 0.9f;
                script.Temperature = 0.1f;
                break;
            case 2:
                // Steel
                script.Roughness = 0.33f;
                script.RoughnessDensity = 0.80f;
                script.BumpSize = 0f;
                script.BumpDensity = 0f;
                script.Hardness = 1f;
                script.Temperature = -0.7f;
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
    [Range(0.0f, 1.0f)] public float RoughnessDensity = 0f;
    [Range(0.0f, 1.0f)] public float BumpSize = 0f;
    [Range(0.0f, 1.0f)] public float BumpDensity = 0f;
    [Range(0.0f, 1.0f)] public float Hardness = 0f;
    [Range(-1.0f, 1.0f)] public float Temperature = 0f;

    public Boolean sendData = false;
    public Boolean activate = false;
    public Boolean deactivate = false;

    void Update()
    {
        // Buttons for testing purposes
        if (sendData){
            CommunicationController.Instance.SendMsg("1," +
                (Temperature * 10 + 30).ToString() + "," +
                Roughness.ToString() + "," +
                BumpSize.ToString());
            sendData = false;
        }
        if (activate){
            CommunicationController.Instance.SendMsg("2," +
                "50," +
                "50," +
                "40");
            activate = false;
        }
        if (deactivate){
            CommunicationController.Instance.SendMsg("0,0,0,0");
            deactivate = false;
        }
    }
}
