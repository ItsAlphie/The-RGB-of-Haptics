using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTracking : MonoBehaviour
{
    [Range(0.0f, 1.0f)] 
    public float flex = 1f;

    // Assign in the inspector
    public GameObject indexBase;
    public GameObject indexMid;
    public GameObject indexTip;

    private readonly float rotationLimit = 90f;

    // Start is called before the first frame update
    void Start(){
        indexBase = this.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        indexMid = indexBase.gameObject.transform.GetChild(0).gameObject;
        indexTip = indexMid.gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        indexBase.transform.localEulerAngles = Vector3.right * flex * rotationLimit / 2;
        indexMid.transform.localEulerAngles = Vector3.right * flex * rotationLimit;
        indexTip.transform.localEulerAngles = Vector3.right * flex * rotationLimit;
    }
}
