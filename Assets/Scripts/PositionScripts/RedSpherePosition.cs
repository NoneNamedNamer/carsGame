using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotToSphere : MonoBehaviour
{
    public Vector3 V3;

    void Update()
    {
        V3 = this.transform.position;
        if (V3.x >= 5.7f && V3.x <= 8.8f &&
            V3.y > 0.0f &&
            V3.z <= 34.2f && V3.z >= 27.6f)
        {
            gameObject.SendMessage("Yo", 5.0f);
        }
    }

    public void Yo(float damage)
    {
        print(damage);
    }
}