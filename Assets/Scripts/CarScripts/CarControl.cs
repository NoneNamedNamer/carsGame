using System.Diagnostics;
using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class CarControl : MonoBehaviour
{
    // Canvases
    [SerializeField] GameObject DamagedCarCanvas;
    [SerializeField] GameObject InterfaceCanvas;
    
    // Values for car mechanics
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;

    // Boolean for checking if UI is active or not
    bool activeUI = false;

    // Texts for UI
    [SerializeField] TMPro.TMP_Text speedText;
    [SerializeField] TMPro.TMP_Text gearText;
    [SerializeField] TMPro.TMP_Text tachText;
    [SerializeField] TMPro.TMP_Text fuelText;

    // Synchronisation
    float sync = 0.0f;
    // Gear. Will have values from -1 to 5. Neutral gear has 0 value
    // Default value is 1
    int gear = 1;
    // Tachometer
    float tach = 1.0f;

    // Value for car's damage
    static float damage = 0.0f;
    public static float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    // Value for car's fuel
    static float fuel = 5000.0f;  
    public static float Fuel
    {
        get { return fuel; }
        set { fuel = value; }
    }

    // Massive of wheels. Using WheelControl script
    WheelControl[] wheels;
    // Object rigidBody for further actions
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        // Finding rigidBody component
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;

        // Find all child GameObjects that have the WheelControl script attached
        wheels = GetComponentsInChildren<WheelControl>();     
    }

    // Massive updating method
    void Update()
    {
        float vInput = Input.GetAxis("Vertical");
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

        // Gear shifting
        // Neutral gear
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.N))
        {
            gear = 0;
            sync = 0;
            // Setting last tachometer value
            tach = speedFactor * 2500;
        }
        // Gear 1
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            sync = 1.0f - damage;
            if (sync < 0.6f && sync >= 0.01f)
            {
                damage += 0.01f;
            }
            else if (gear > 2)
            {
                damage += 0.02f;
            }
            else
            {
                maxSpeed = 20;
                gear = 1;
                tach = speedFactor * 2500;
            }            
        }
        // Gear 2
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            sync = speedFactor + 1.0f - damage;
            if (sync < 1.5f)
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
                tach = speedFactor * 2500;
            }
        }
        // Gear 3
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            sync = speedFactor + 2.0f - damage;
            if (sync < 2.4f)
            {
                damage += 0.01f;
            }
            else if ((gear > 4 || gear < 2) && gear != 0)
            {
                damage += 0.02f;
            }
            else
            {                
                maxSpeed = 60;
                gear = 3;
                tach = speedFactor * 2500;
            }
        }
        // Gear 4
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha4))
        {
            sync = speedFactor + 3.0f - damage;
            if (sync < 3.3f)
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
                tach = speedFactor * 2500;
            }
        }
        // Gear 5
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5))
        {
            sync = speedFactor + 4.0f - damage;
            if (sync < 4.2f)
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
                tach = speedFactor * 2500;
            }
        }
        // Reverse gear
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            sync = speedFactor;
            if (sync > 0.01f)
            {
                damage += 0.01f;
            }
            else if (gear > 1)
            {
                damage += 0.05f;
            }
            else
            {
                maxSpeed = 40;
                gear = -1;
                tach = speedFactor * 2500;
            }
        }

        // Tachometer in UI
        // Setting it after gear shifting for setting correct value
        // of tachometer in UI
        tach = speedFactor * 2500;

        // Checking if car's damage is higher than valid value
        if (damage > 0.2f)
        {
            DamagedCarCanvas.SetActive(true);
        }
        else
        {
            DamagedCarCanvas.SetActive(false);
        }

        // Wheel management
        foreach (var wheel in wheels)
        {
            // Deactivating motorization for car if
            // car has neutral gear or has fuel capacity lower than 50
            if (gear == 0 || fuel <= 50)
            {
                wheel.motorized = false;
            }
            // Condition for motorization car if gear differs from neutral
            else if (gear != 0)
            {
                wheel.motorized = true;
            }

            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }
            
            // Checking if car moves there where it drives
            if (isAccelerating)
            {                
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                    fuel = fuel - Mathf.Abs(vInput)/100;
                }
                // Non-active brakes
                wheel.WheelCollider.brakeTorque = 0;
            }
            // Opposite checking
            if (!isAccelerating)
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }

        // Adjusting key Tab to turn on/off car's UI
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            activeUI = !activeUI;
            InterfaceCanvas.SetActive(activeUI);
        }

        // UI text info
        speedText.text = $"Speed: {Math.Round(Mathf.Abs(forwardSpeed), 0)}";
        gearText.text = $"Gear: {gear}";
        tachText.text = $"Tach: {Math.Round(tach, 0)}";
        fuelText.text = $"Fuel: {Math.Round(fuel)}";
    }
}