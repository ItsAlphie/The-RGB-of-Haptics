using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchProcessor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched object: " + other.gameObject.name);

        HapticInfo hapticInfo = other.gameObject.GetComponent<HapticInfo>();
        if (hapticInfo != null)
        {
            Debug.Log("Haptic Info of " + other.gameObject.name + ": Roughness " + hapticInfo.Roughness +
                ", BumpsSize " + hapticInfo.BumpsSize + ", Temperature " + hapticInfo.Temperature);
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Get speed information
    }

    void OnTriggerExit(Collider other)
    {
        // Send STOP over UDP
    }
}
