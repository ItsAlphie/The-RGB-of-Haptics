using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveData : MonoBehaviour
{
    public Button quitButton;
    public GameObject panels;

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
        // TODO: differentiate between test types
        for (int i = 0; i < panels.transform.childCount; i++)
        {   
            GameObject panel = panels.transform.GetChild(i).GetChild(1).gameObject;
            GameObject test = panel.gameObject.transform.GetChild(1).GetChild(3).
                GetChild(0).GetChild(0).gameObject;

            List<int> testValues = new List<int>();
            for (int j = 0; j < test.transform.childCount; j++)
            {
                GameObject question = test.transform.GetChild(j).gameObject;
                int testValue = GetDropdownValue(question);
                testValues.Add(testValue);
            }

            // TODO: Save lists naar file
        }
    }

    int GetDropdownValue(GameObject dropdown)
    {
        int value = dropdown.GetComponent<TMPro.TMP_Dropdown>().value;
        print("Dropdown value: " + value);
        return value;
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
