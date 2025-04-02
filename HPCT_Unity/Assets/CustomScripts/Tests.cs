using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Tests : MonoBehaviour
{   
    [SerializeField] GameObject parentObject;
    [SerializeField] GameObject[] tests;
    [SerializeField] Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(-0.094f, -0.057f, -0.019f),
        new Vector3(-1.625f, -0.057f, -0.019f),
        new Vector3(-3.167f, -0.05f, -0.019f),
        new Vector3(2.982f, -0.053f, 4.466f),  
        new Vector3(2.982f, -0.053f, 2.888f), 
        new Vector3(2.982f, -0.053f, 5.862f),
        new Vector3(14.6739998,-0.0570001602,-6.14400005),
        new Vector3(13.1890001,-0.0570001602,-6.14400005),
        new Vector3(11.6269999,-0.0570001602,-6.14400005),
        new Vector3(-5.92000008,-0.0570001602,-3.82599998),
        new Vector3(-4.4380002,-0.0570001602,-3.82599998),
        new Vector3(-2.99499989,-0.0570001602,-3.82599998),
        new Vector3(-1.47800004,-0.0570001602,-3.82599998),
        new Vector3(9.27000046,-0.0570001602,0.495999992)
    };
     [SerializeField] Quaternion[] spawnRotations = new Quaternion[]
    {
        Quaternion.Euler(0.0f, 0.0f, 0.0f),
        Quaternion.Euler(0.0f, 0.0f, 0.0f),
        Quaternion.Euler(0.0f, 0.0f, 0.0f),
        Quaternion.Euler(0.0f, 90.0f, 0.0f),
        Quaternion.Euler(0.0f, 90.0f, 0.0f),
        Quaternion.Euler(0.0f, 90.0f, 0.0f),
        Quaternion.Euler(0.0f, 180.0f, 0.0f),
        Quaternion.Euler(0.0f, 180.0f, 0.0f),
        Quaternion.Euler(0.0f, 180.0f, 0.0f),
        Quaternion.Euler(0.0f, 0.0f, 0.0f),
        Quaternion.Euler(0.0f, 0.0f, 0.0f),
        Quaternion.Euler(0.0f, 0.0f, 0.0f),
        Quaternion.Euler(0.0f, 0.0f, 0.0f),
        Quaternion.Euler(0.0f, 90.0f, 0.0f),
    };
    
    // Start is called before the first frame update
    void Start()
    {
        spawnTests();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // fills the list of integers that represent each test, and shuffles it for randomized order
    private int[] fillAndShuffleList(){
        
        int[] temp = new int[tests.Length];
        for (int i = 0; i < tests.Length; i++)
        {
            temp[i] = i;
        }
        return ShuffleArray(temp);
    }

    // Shuffle method from https://thomassteffen.medium.com/super-simple-array-shuffle-with-linq-167b317ba035
    static T[] ShuffleArray<T>(T[] array)
    {
        System.Random random = new System.Random();
        return array.OrderBy(x => random.Next()).ToArray();
    }

    private void spawnTests(){
        int[] testsOrder =  fillAndShuffleList();

        foreach(int i in testsOrder){
            Debug.Log(i);
        }
        for (int i = 0; i < tests.Length; i++){
        Debug.Log("at index "+ i);
            GameObject test = Instantiate(tests[testsOrder[i]], this.transform);
            test.transform.localPosition = spawnPositions[i];
            test.transform.localRotation = spawnRotations[i];
        }
    }

}
