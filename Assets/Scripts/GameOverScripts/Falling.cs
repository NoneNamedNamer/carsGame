using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Falling : MonoBehaviour
{
    Vector3 V3;
    int q = -7;

    void Update()
    {
        V3 = this.transform.position;
        if (V3.y < q)
        {
            SceneManager.LoadScene(2);
        }
    }
}