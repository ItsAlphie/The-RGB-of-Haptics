using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchProcessor : MonoBehaviour
{
    private Vector3 previousPosition;
    private Vector3 velocity;
    private Queue<float> velocitySamples = new Queue<float>();
    private const int sampleSize = 20;

    void Start()
    {
        previousPosition = transform.position;
    }

    /* The finger's velocity is smoothed by averaging the latest values
     */
    void Update()
    {
        velocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;

        velocitySamples.Enqueue(velocity.magnitude);
        if (velocitySamples.Count > sampleSize)
        {
            velocitySamples.Dequeue();
        }
    }

    /* When the finger touches an object
     * gather haptic information from the object
     * send it over UDP to microcontroller
     */
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched object: " + other.gameObject.name);

        HapticInfo hapticInfo = other.gameObject.GetComponent<HapticInfo>();
        if (hapticInfo != null)
        {
            Debug.Log("Haptic Info of " + other.gameObject.name + ": Roughness " + hapticInfo.Roughness +
                ", BumpsSize " + hapticInfo.BumpsSize + ", Temperature " + hapticInfo.Temperature);
            CommunicationController.Instance.SendMsg("1," + hapticInfo.Roughness.ToString() + "," + 
                hapticInfo.BumpsSize.ToString() + "," + hapticInfo.Temperature.ToString());
        }
    }

    /* Send finger's calculated velocity 
     * over UDP to microcontroller
     */
    void OnTriggerStay(Collider other)
    {
        HapticInfo hapticInfo = other.gameObject.GetComponent<HapticInfo>();
        if (hapticInfo != null)
        {
            float averageVelocity = CalculateAverageVelocity();
            Debug.Log("Finger Speed: " + averageVelocity);
            CommunicationController.Instance.SendMsg("2," + averageVelocity.ToString("F3"));
        }
    }

    void OnTriggerExit(Collider other)
    {
        HapticInfo hapticInfo = other.gameObject.GetComponent<HapticInfo>();
        if (hapticInfo != null)
        {
            CommunicationController.Instance.SendMsg("0");
        }
    }

    private float CalculateAverageVelocity()
    {
        float sum = 0f;
        foreach (float sample in velocitySamples)
        {
            sum += sample;
        }
        return sum / velocitySamples.Count;
    }
}
