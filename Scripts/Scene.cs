using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    } 
    public void BackToReady()
    {
        SceneManager.LoadScene(1);
    }
    public void Load()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
