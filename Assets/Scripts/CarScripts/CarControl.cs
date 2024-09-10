using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    public GameObject DamagedCarCanvas;
    
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;

    float sync = 0.0f;
    int gear = 1;

    public float damage = 0.0f;

    static float fuel = 5000.0f;  

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
        print(fuel);
        float vInput = Input.GetAxis("Vertical");
        //Debug.Log(vInput);
        float hInput = Input.GetAxis("Horizontal");

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.velocity);

        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);      

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
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            sync = speedFactor - damage;
            if (gear > 2)
            {
                damage += 0.02f;
            }
            else
            {
                maxSpeed = 20;
                gear = 1;
            }            
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            sync = speedFactor + 1.0f - damage;
            if (sync < 1.4f)
            {
                damage += 0.01f;
            }
            else if (gear > 3)
            {
                damage += 0.02f;
            }
            else
            {                
                maxSpeed = 40;
                gear = 2;
            }
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            sync = speedFactor + 2.0f - damage;
            if (sync < 2.4f)
            {
                damage += 0.01f;
            }
            else if (gear > 4 || gear < 2)
            {
                damage += 0.02f;
            }
            else
            {                
                maxSpeed = 60;
                gear = 3;
            }
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha4))
        {
            sync = speedFactor + 3.0f - damage;
            if (sync < 3.4f)
            {
                damage += 0.01f;
            }
            else if (gear < 3)
            {
                damage += 0.02f;
            }
            else
            {                
                maxSpeed = 80;
                gear = 4;
            }
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5))
        {
            sync = speedFactor + 4.0f - damage;
            if (sync < 4.4f)
            {
                damage += 0.01f;
            }
            else if (gear < 4)
            {
                damage += 0.02f;
            }
            else
            {                
                maxSpeed = 90;
                gear = 5;
            }
        }
        
        if (damage > 0.2f)
        {
            DamagedCarCanvas.SetActive(true);
        }
        else
        {
            DamagedCarCanvas.SetActive(false);
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