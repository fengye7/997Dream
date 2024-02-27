using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStage : MonoBehaviour
{
    public List<GameObject> shop;
    void Start()
    {
        for (int i = 0; i < shop.Count; i++)
        {
            int randomNum = Random.Range(0, 3);
            if(randomNum <= 1)
            {
                shop[i].GetComponent<Statecheck>()._state = false;
            }
            else
            {
                shop[i].GetComponent<Statecheck>()._state = true;
            }
        }
    }
    void Update()
    {
        
    }
}
