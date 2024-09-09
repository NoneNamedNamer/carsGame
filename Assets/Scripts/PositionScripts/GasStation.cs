using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotToSphereGS1 : MonoBehaviour
{
    Vector3 V3;
    float gs = 0.0f;
    float fuelTank = 0.0f;

    void Update()
    {
        V3 = this.transform.position;
        if (V3.x >= 381.11f && V3.x <= 381.4905f &&
            V3.y > 0.0f &&
            V3.z <= 104.33f && V3.z >= 97.8f)
        {
            gameObject.SendMessage("Refueling", fuelTank);
            
        }
        
        if (V3.x >= 371.8f && V3.x <= 372.3f &&
            V3.y > 0.0f &&
            V3.z <= 104.33f && V3.z >= 97.8f)
        {
            gameObject.SendMessage("Refueling", fuelTank);
            
        }
    }

    public void Refueling(float fuel)
    {
        if (gs < 10000.0f)
        {
            print(gs + fuel);
            gs++;
        }
        else
        {
            print("Done.");
        }        
    }
}