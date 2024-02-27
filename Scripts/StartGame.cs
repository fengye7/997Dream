using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Click() 
    { 
        StartCoroutine(ScaleChange());
    }
    IEnumerator ScaleChange() 
    { 
        transform.DOScale(0.9f, 0.15f);
        yield return new WaitForSeconds(0.15f);
        transform.DOScale(1f, 0.15f); 
    }


}
