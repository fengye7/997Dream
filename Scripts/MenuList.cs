using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuList : MonoBehaviour
{
    public GameObject menuList;//菜单列表

    [SerializeField] private bool menukeys = true;


    // Update is called once per frame
    void Update()
    {
        if(menukeys)
        { 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuList.SetActive(true);
                menukeys = false;
                Time.timeScale = 0;//时间暂停
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuList.SetActive(false);
            menukeys = true;
            Time.timeScale = 1;//时间恢复
        }
    }
    public void Return()
    {
        menuList.SetActive(false);
        menukeys = true;
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void Exit()
    {
        Application.Quit();
    }
}



     
