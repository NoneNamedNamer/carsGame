using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FuelManagement : MonoBehaviour
{
    // Vector for getting active position
    Vector3 aPos;

    // Canvases
    [SerializeField] GameObject RefuelingCanvas;   
    [SerializeField] GameObject RefuelingDoneCanvas;
    [SerializeField] GameObject LowFuelWarningCanvas;

    // Refueling music. It plays while player refuels
    [SerializeField] GameObject AudioSource;

    // Updating checks for activating methods if needed
    void Update()
    {
        aPos = this.transform.position;
        if (aPos.x >= 381.11f && aPos.x <= 381.4905f &&
            aPos.y > 0.0f &&
            aPos.z <= 104.33f && aPos.z >= 97.8f)
        {            
            Refueling();
        }
        
        if (aPos.x >= 371.8f && aPos.x <= 372.3f &&
            aPos.y > 0.0f &&
            aPos.z <= 104.33f && aPos.z >= 97.8f)
        {           
            Refueling();
        }
        LowFuel();        
    }

    // Method for refueling. Checks value fuel from class CarContol
    public void Refueling()
    {
        if (CarControl.Fuel < 10000.0f)
        {
            if (CarControl.Fuel < 9900.0f)
            {
                RefuelingCanvas.SetActive(true);
            }           
            AudioSource.SetActive(true);
            CarControl.Fuel++;
        }
        else
        {
            RefuelingCanvas.SetActive(false);
            AudioSource.SetActive(false);
            RefuelingDoneCanvas.SetActive(true);
        }        
    }

    // Method for showing warning if fuel is low
    public void LowFuel()
    {
        if (CarControl.Fuel < 2000.0f)
        {
            LowFuelWarningCanvas.SetActive(true);
        }
        else
        {
            LowFuelWarningCanvas.SetActive(false);
        }
    }
}