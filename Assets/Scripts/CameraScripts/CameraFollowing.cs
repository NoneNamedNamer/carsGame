using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(transform.parent);
    }
}