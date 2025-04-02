using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Xml.Linq;

public class SaveData : MonoBehaviour
{
    public Button quitButton;
    public bool test1 = true;
    private string xmlFilePath;
    private XDocument xmlDoc;
    int userID = 1;
    bool gotFile1 = false;
    bool gotFile2 = false;

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
        if (test1)
        {
            SaveData1();
            SceneManager.LoadScene("MaterialTests");
        }
        /*else
        {
            SaveData2();
            QuitGame();
        }*/
    }

    void SaveData1()
    {
        List<string> testNames = new List<string>
        {
            "Test1A(Clone)",
            "Test1B_1(Clone)",
            "Test1B_2(Clone)",
            "Test1C(Clone)",
            "Test1D(Clone)",
            "Test1E(Clone)"
        };

        foreach (string testName in testNames)
        {
            GameObject testObject = GameObject.Find(testName);
            if (testObject != null)
            {
                Debug.Log("Saving results of " + testName);
                GameObject content = testObject.gameObject.transform.GetChild(1).GetChild(1).GetChild(3).
                GetChild(0).GetChild(0).gameObject;

                List<int> testValues = new List<int>();
                for (int j = 0; j < content.transform.childCount; j++)
                {
                    GameObject question = content.transform.GetChild(j).GetChild(1).gameObject;
                    int testValue = GetDropdownValue(question);
                    testValues.Add(testValue);
                }

                SaveTestResultsToXml(testName, testValues);
            }
            else
            {
                Debug.LogWarning("GameObject not found: " + testName);
            }
        }
    }

    void SaveData2()
    {
        // Implement SaveData2 logic if needed
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

    void SaveTestResultsToXml(string testName, List<int> testValues)
    {
        while ((test1 & !gotFile1) || (!test1 & !gotFile2))
        {
            if (test1){xmlFilePath = "Test1.xml";}
            else{xmlFilePath = "Test2.xml"; }

            if (System.IO.File.Exists("TestResults/"+userID+"_"+xmlFilePath))
            {
                userID++;
            }
            else
            {
                xmlDoc = new XDocument(new XElement("TestResults"));
                xmlFilePath = "TestResults/" + userID + "_" + xmlFilePath;
                if(test1){gotFile1 = true;}
                else{gotFile2 = true;}
                break;
            }
        }

        XElement testElement = new XElement("Test",
            new XAttribute("name", testName),
            new XElement("Values", string.Join(",", testValues))
        );

        xmlDoc.Root.Add(testElement);
        xmlDoc.Save(xmlFilePath);
    }
}
