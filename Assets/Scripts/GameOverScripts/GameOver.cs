using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //public GameObject GameOverCanvas;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CarPlayer")
        {
            SceneManager.LoadScene(2);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
    public void GetToTheMainMenu()
    {
        SceneManager.LoadScene(0);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}