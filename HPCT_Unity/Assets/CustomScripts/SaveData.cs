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
        else
        {
            SaveData2();
            QuitGame();
        }
    }

    void SaveData1()
    {
        List<string> testNames = new List<string>
        {
            "Test1A_1(Clone)",
            "Test1A_3(Clone)",
            "Test1B_1(Clone)",
            "Test1B_3(Clone)",
            "Test1C_2(Clone)",
            "Test1C_4(Clone)",
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
                for (int i = 0; i < content.transform.childCount; i++)
                {
                    GameObject question = content.transform.GetChild(i).GetChild(1).gameObject;
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
        List<string> testNames = new List<string>
        {
            "Water Bowl Test",
            "Metal Test",
            "Wood Test"
        };

        foreach (string testName in testNames)
        {
            GameObject testObject = GameObject.Find(testName);
            if (testObject != null)
            {
                Debug.Log("Saving results of " + testName);
                GameObject panels = testObject.gameObject.transform.GetChild(0).gameObject;

                List<List<int>> testValues = new List<List<int>>();
                for (int i = 0; i < panels.transform.childCount; i++)
                {
                    GameObject panel = panels.transform.GetChild(i).gameObject;
                    GameObject content = panel.gameObject.transform.GetChild(1).GetChild(3).
                    GetChild(0).GetChild(0).gameObject;

                    List<int> panelValues = new List<int>();
                    for (int j = 0; j < content.transform.childCount; j++)
                    {
                        GameObject question = content.transform.GetChild(j).GetChild(1).gameObject;
                        int testValue = GetDropdownValue(question);
                        panelValues.Add(testValue);
                    }
                    testValues.Add(panelValues);
                }

                SaveTestResultsToXml(testName, testValues);
            }
            else
            {
                Debug.LogWarning("GameObject not found: " + testName);
            }
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

    void SaveTestResultsToXml(string testName, List<int> testValues)
    {
        // Save data for test 1
        while (!gotFile1)
        {
            xmlFilePath = "Test1.xml";

            if (System.IO.File.Exists("TestResults/"+userID+"_"+xmlFilePath))
            {
                userID++;
            }
            else
            {
                xmlDoc = new XDocument(new XElement("TestResults"));
                xmlFilePath = "TestResults/" + userID + "_" + xmlFilePath;
                gotFile1 = true;
                break;
            }
        }

        XElement testElement = new XElement("Test",
            new XAttribute("name", testName),
            ArrayToElements(testValues)
        );

        xmlDoc.Root.Add(testElement);
        xmlDoc.Save(xmlFilePath);
    }

    void SaveTestResultsToXml(string testName, List<List<int>> testValues)
    {
        // Save data for test 2
        while (!gotFile2)
        {
            xmlFilePath = "Test2.xml";

            if (System.IO.File.Exists("TestResults/" + userID + "_" + xmlFilePath))
            {
                userID++;
            }
            else
            {
                xmlDoc = new XDocument(new XElement("TestResults"));
                xmlFilePath = "TestResults/" + userID + "_" + xmlFilePath;
                gotFile2 = true;
                break;
            }
        }

        XElement testElement = new XElement("Test",
            new XAttribute("name", testName),
            ArrayToElements(testValues)
        );

        xmlDoc.Root.Add(testElement);
        xmlDoc.Save(xmlFilePath);
    }

    XElement ArrayToElements(List<int> testValues)
    {
        XElement values = new XElement("Values");
        foreach (int value in testValues)
        {
            values.Add(new XElement("Value", value));
        }
        return values;
    }

    XElement ArrayToElements(List<List<int>> testValues)
    {
        XElement values = new XElement("Values");
        foreach (List<int> panel in testValues)
        {
            foreach (int value in panel)
            {
                values.Add(new XElement("Value", value));
            }
        }
        return values;
    }
}
