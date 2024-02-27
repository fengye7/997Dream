using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlant : MonoBehaviour
{
    
    void Start()
    {
        GetComponent<ItemController>().AddItem();
        GetComponent<ItemController>().DestroyItem(gameObject);
    }

    
    void Update()
    {
        
    }
   
}
