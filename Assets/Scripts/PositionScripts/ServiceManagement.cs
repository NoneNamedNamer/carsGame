using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceManagement : MonoBehaviour
{
    Vector3 V3;

    [SerializeField]
    GameObject RepairingDoneCanvas;
    //public GameObject AudioSource;

    // Update is called once per frame
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

    public void Repairing()
    {
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

        if (CarControl.Damage == 0.0f)
        {
            RepairingDoneCanvas.SetActive(true);
        }
    }
}
