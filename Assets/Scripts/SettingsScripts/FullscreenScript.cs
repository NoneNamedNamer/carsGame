using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            if (Screen.fullScreen == true)
            {
                SetFullscreen(false);
            }
            else
            {
                SetFullscreen(true);
            }

        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
