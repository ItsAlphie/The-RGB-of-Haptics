using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchProcessor : MonoBehaviour
{
    private Vector3 previousPosition;
    private Vector3 velocity;
    private Queue<float> velocitySamples = new Queue<float>();
    private const int sampleSize = 20;
    private float maxVelocity = 1.5f;

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
            int roughness = (int)(hapticInfo.Roughness * 255);
            int bumpSize = (int)(hapticInfo.BumpSize * 255);
            int temperature = (int)(hapticInfo.Temperature * 10 + 30);

            Debug.Log("Haptic Info of " + other.gameObject.name + ": " +
                "Temperature " + temperature + ", " +
                "Roughness " + roughness + ", " +
                "BumpsSize " + bumpSize);
            CommunicationController.Instance.SendMsg("1," + 
                temperature.ToString() + "," + 
                roughness.ToString() + "," + 
                bumpSize.ToString());
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
            // Linearly scaling the frequency of bumps and roughness based on the finger's velocity
            float averageVelocity = CalculateAverageVelocity();

            int bumpsFrequency = (int)(hapticInfo.BumpDensity * 
                100 * averageVelocity / maxVelocity);
            int roughnessFrequency = (int)(hapticInfo.RoughnessDensity * 
                100 * averageVelocity / maxVelocity);

            // P-controller to adjust the servo angle based on the depth of the finger
            /*
            float colliderDistance = Vector3.Distance(transform.position, 
                other.ClosestPoint(transform.position));
            float depth = 1 - (10000 * (colliderDistance)) / 237;
            
            int goalAngle = (int) (180 * depth);
            int currentAngle = hapticInfo.getActualAngle();
            int angleOffset = goalAngle - currentAngle;

            int servoAngle = (int)(currentAngle + angleOffset * ((hapticInfo.Hardness + 0.1) / 1.1)); */

            // Adjustable force range: Harder material = shorter range
            float colliderDistance = Vector3.Distance(transform.position,
                other.ClosestPoint(transform.position));
            float depth = 1 - (10000 * (colliderDistance)) / 237;
            float maxDepth = 1.001f - hapticInfo.Hardness;

            int servoAngle = (int)(180 * (depth / maxDepth));
            if (servoAngle > 180){
                servoAngle = 180;
            }
            else if (servoAngle < 0){
                servoAngle = 0;
            }

            Debug.Log("New Servo Angle: " + servoAngle);
            CommunicationController.Instance.SendMsg("2," + 
                roughnessFrequency.ToString() + "," + 
                bumpsFrequency.ToString() + "," + 
                servoAngle.ToString());
        }
    }

    void OnTriggerExit(Collider other)
    {
        HapticInfo hapticInfo = other.gameObject.GetComponent<HapticInfo>();
        if (hapticInfo != null)
        {
            CommunicationController.Instance.SendMsg("0,0,0,0");
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
