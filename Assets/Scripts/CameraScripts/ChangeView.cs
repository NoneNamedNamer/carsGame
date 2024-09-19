using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeView : MonoBehaviour
{
    public Vector3 V3, VV; 

    void Changing()
    {
        V3 = new Vector3(0.0f, 0.5f, 1.7f);
        VV = this.transform.position;
        if (Input.GetKeyDown(KeyCode.C))
        {
            VV = V3;
        }
    }
}
