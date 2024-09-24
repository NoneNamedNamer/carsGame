using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FuelManagement : MonoBehaviour
{
    Vector3 V3;

    [SerializeField]
    GameObject RefuelingCanvas;
    
    [SerializeField]
    GameObject RefuelingDoneCanvas;

    [SerializeField]
    GameObject LowFuelWarningCanvas;

    [SerializeField]
    GameObject AudioSource;

    void Update()
    {
        V3 = this.transform.position;
        if (V3.x >= 381.11f && V3.x <= 381.4905f &&
            V3.y > 0.0f &&
            V3.z <= 104.33f && V3.z >= 97.8f)
        {            
            Refueling();
        }
        
        if (V3.x >= 371.8f && V3.x <= 372.3f &&
            V3.y > 0.0f &&
            V3.z <= 104.33f && V3.z >= 97.8f)
        {           
            Refueling();
        }

        LowFuel();        
    }

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