using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarChoosing : MonoBehaviour
{
    public void ChoiceDefault()
    {
        SceneManager.LoadScene(1);
    }

    public void ChoiceTaxi()
    {
        SceneManager.LoadScene(3);
    }
}
