using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveData : MonoBehaviour
{
    public Button quitButton;
    public GameObject[] panels;

    void Start()
    {
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(FinishTest);
        }
        else
        {
            Debug.LogError("Quit button not assigned");
        }
    }

    void FinishTest()
    {
        SaveUserData();
        QuitGame();
    }

    void SaveUserData()
    {
        /*/ TODO: differentiate between test types
        foreach (GameObject panel in panels)
        {
            GameObject test = this.gameObject.transform.GetChild(1).GetChild(3).
                GetChild(0).GetChild(0).gameObject;
            List<int> testValues = new List<int>();
            foreach (GameObject question in test.transform.gameObject)
            {
                testValues.Add(GetDropdownValue(question.gameObject));
            }


            // Save values naar lists
            // Save lists naar file
        }
        // Fix list van panels
        // Get values van elke panel
        // Save values naar lists
        // Save lists naar file*/
    }

    void GetDropdownValue(GameObject dropdown)
    {
        /*/ Get value from 'Dropdown - TextMeshPro' attached to dropdown gameobject
        int value = dropdown.GetComponent("Dropdown - TextMeshPro").value;
        print("Dropdown value: " + value);
        return value;*/
    }

    void QuitGame()
    {
        Debug.Log("Exiting Game Automatically");

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
