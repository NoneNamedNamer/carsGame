using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResettingPosition : MonoBehaviour
{
    public Vector3 V3;

    void Start()
    {
        V3 = new Vector3(-396.7775f, -475.3808f, 21.53146f);
    }

    public void Resetting()
    {
        SceneManager.LoadScene(1);
    }
}