using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceManagement : MonoBehaviour
{
    Vector3 V3;
    // Canvases
    [SerializeField] GameObject RepairingDoneCanvas;
    //public GameObject AudioSource;

    // Updating checks for activating methods if needed
    void Update()
    {
        V3 = this.transform.position;
        if (V3.x >= 88.61537f && V3.x <= 93.21815f &&
            V3.y > 0.0f &&
            V3.z <= 406.1149f && V3.z >= 400.0f)
        {
            Repairing();
        }

        if (V3.x >= 88.6263f && V3.x <= 93.23714f &&
            V3.y > 0.0f &&
            V3.z <= 410.3726f && V3.z >= 406.1322f)
        {
            Repairing();
        }
    }

    // Method for repairing car
    public void Repairing()
    {
        // Check if damage more than value 0.
        // If so by pressing key "R" car will be repaired immediately
        if (CarControl.Damage > 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                CarControl.Damage = 0.0f;
            }           
            //AudioSource.SetActive(true);
        }
        else
        {
            //AudioSource.SetActive(false);
            //RepairingDoneCanvas.SetActive(true);
        }

        // Check for checking if damage's value equals 0.
        // If so canvas of finished repairing will be shown on the screen
        // while player is on exact position for activating
        if (CarControl.Damage == 0.0f)
        {
            RepairingDoneCanvas.SetActive(true);
        }
    }
}
