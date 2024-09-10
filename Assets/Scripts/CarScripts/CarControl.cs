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

    float checker = 0.0f;
    int rechecker = 1;

    public float damage = 0.0f;

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

        //Gear shifting
        if (Input.GetKeyDown(KeyCode.F1))
        {
            checker = speedFactor - damage;
            if (rechecker > 2)
            {
                damage += 0.05f;
            }
            else
            {
                maxSpeed = 20;
                rechecker = 1;
            }            
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            checker = speedFactor + 1.0f - damage;
            if (checker < 1.4f)
            {
                damage += 0.01f;
            }
            else if (rechecker > 3)
            {
                damage += 0.05f;
            }
            else
            {                
                maxSpeed = 40;
                rechecker = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            checker = speedFactor + 2.0f - damage;
            if (checker < 2.4f)
            {
                damage += 0.01f;
            }
            else if (rechecker > 4 || rechecker < 2)
            {
                damage += 0.05f;
            }
            else
            {                
                maxSpeed = 60;
                rechecker = 3;
            }
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            checker = speedFactor + 3.0f - damage;
            if (checker < 3.4f)
            {
                damage += 0.01f;
            }
            else if (rechecker < 3)
            {
                damage += 0.05f;
            }
            else
            {                
                maxSpeed = 80;
                rechecker = 4;
            }
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            checker = speedFactor + 4.0f - damage;
            if (checker < 4.4f)
            {
                damage += 0.01f;
            }
            else if (rechecker < 4)
            {
                damage += 0.05f;
            }
            else
            {                
                maxSpeed = 90;
                rechecker = 5;
            }
        }
        print(checker);

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
                    //print(fuel);
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