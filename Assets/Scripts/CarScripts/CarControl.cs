using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;

    static float fuel = 10000.0f;

    public static float Fuel
    {
        get { return fuel; }
        set { fuel = value; }
    }

    WheelControl[] wheels;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;

        // Find all child GameObjects that have the WheelControl script attached
        wheels = GetComponentsInChildren<WheelControl>();     
    }

    // Update is called once per frame
    void Update()
    {
        float vInput = Input.GetAxis("Vertical");
        //Debug.Log(vInput);
        float hInput = Input.GetAxis("Horizontal");

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.velocity);

        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        //print(speedFactor);       

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // …and to calculate how much to steer 
        // (the car steers more gently at top speed)
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        // as the car's velocity
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        if (Input.GetKeyDown(KeyCode.F1))
        {
            maxSpeed = 20;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            maxSpeed = 40;
            if (speedFactor < 0.5f)
            {
                maxSpeed = 20;
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            maxSpeed = 60;
            if (speedFactor < 0.5f)
            {
                maxSpeed = 40;
            }
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            maxSpeed = 80;
            if (speedFactor < 0.5f)
            {
                maxSpeed = 60;
            }
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            maxSpeed = 90;
            if (speedFactor < 0.5f)
            {
                maxSpeed = 80;
            }
        }

        foreach (var wheel in wheels)
        {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }
            
            if (isAccelerating)
            {
                
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                    fuel = fuel - Mathf.Abs(vInput)/100;
                    print(fuel);
                }

                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }
}