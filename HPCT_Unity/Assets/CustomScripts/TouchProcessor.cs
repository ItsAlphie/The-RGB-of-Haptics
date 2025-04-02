using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchProcessor : MonoBehaviour
{
    private Vector3 previousPosition;
    private Vector3 velocity;
    private Queue<float> velocitySamples = new Queue<float>();
    private const int sampleSize = 15;
    private int counter = 0;
    private float maxVelocity = 1.2f;

    [SerializeField] int[] hardnessSettings_0 = {180};  // Hard
    [SerializeField] int[] hardnessSettings_1 = {20, 90, 180};  // Medium
    [SerializeField] int[] hardnessSettings_2 = {20, 60, 120, 180};  // Soft



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

            int repeat = 2;
            while (repeat > 0)
            {
                CommunicationController.Instance.SendMsg("1," +
                    temperature.ToString() + "," +
                    roughness.ToString() + "," +
                    bumpSize.ToString());
                repeat -= 1;
            }
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
            while (counter < sampleSize)
            {
                counter += 1;
                break;
            }
            counter = 0;
            // Linearly scaling the frequency of bumps and roughness based on the finger's velocity
            float averageVelocity = CalculateAverageVelocity();
            int bumpsFrequency = 0;
            int roughnessFrequency = 0;
            if (averageVelocity >= maxVelocity * 4/3)
            {
                bumpsFrequency = (int)(hapticInfo.BumpDensity) * 9;
                roughnessFrequency = (int)(hapticInfo.RoughnessDensity) * 150;
            }
            else if (averageVelocity > (maxVelocity) * 0.55)
            {
                bumpsFrequency = (int)(hapticInfo.BumpDensity) * 6;
                roughnessFrequency = (int)(hapticInfo.RoughnessDensity) * 100;
            }
            else if (averageVelocity > (maxVelocity) * 0.15)
            {
                bumpsFrequency = (int)(hapticInfo.BumpDensity) * 3;
                roughnessFrequency = (int)(hapticInfo.RoughnessDensity) * 50;
            }
            else
            {
                bumpsFrequency = 0;
                roughnessFrequency = 0;
            }

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
            int servoAngle = 0;
            
            if (depth < 0)
            {
                depth = - depth;
            }

            if (hapticInfo.Hardness == 1) // Hard
            {
                
                servoAngle = hardnessSettings_0[0];
            }
            else if (hapticInfo.Hardness == 0.5) // Medium
            {
                
                if (depth < 0.33f)
                {
                    servoAngle = hardnessSettings_1[0]; 
                }
                else if (depth < 0.66f)
                {
                    servoAngle = hardnessSettings_1[1]; 
                }
                else
                {
                    servoAngle =  hardnessSettings_1[2]; 
                }

            }
            else if (hapticInfo.Hardness == 0) // Soft 
            {
                
                if (depth < 0.25f)
                {
                    servoAngle = hardnessSettings_2[0];
                }
                else if (depth < 0.5f)
                {
                    servoAngle = hardnessSettings_2[1];
                }
                else if (depth < 0.75f)
                {
                    servoAngle = hardnessSettings_2[2];
                }
                else
                {
                    servoAngle = hardnessSettings_2[3]; 
                }
            }

            Debug.Log("New Servo Angle: " + (180-servoAngle));
            CommunicationController.Instance.SendMsg("2," + 
                roughnessFrequency.ToString() + "," + 
                bumpsFrequency.ToString() + "," + 
                (180-servoAngle).ToString());
        }
    }

    void OnTriggerExit(Collider other)
    {
        HapticInfo hapticInfo = other.gameObject.GetComponent<HapticInfo>();
        if (hapticInfo != null)
        {
            int repeat = 3;
            while (repeat > 0)
            {
                CommunicationController.Instance.SendMsg("0,0,0,0");
                repeat -= 1;
            }
            velocitySamples.Clear();
        }
    }

    private float CalculateAverageVelocity()
    {
        float sum = 0f;
        foreach (float sample in velocitySamples)
        {
            sum += sample;
        }
        return sum;
    }
}
