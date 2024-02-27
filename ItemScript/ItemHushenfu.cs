using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHushenfu : MonoBehaviour
{
    public List<GameObject> fuList;
    public int fuNum;
    bool isOver;
    void Start()
    {
        for (int i = 0; i < fuList.Count; i++)
        {
            GameObject fupart = Instantiate(fuList[i].gameObject, new Vector3( Random.Range(-13f, 10f),-0.6f, 0f), Quaternion.identity);
            fupart.GetComponent<FuPart>().fu = gameObject;
        }
    }

   
    void Update()
    {
        if(!isOver && fuNum == 3)
        {
            isOver = true;
            GetComponent<ItemController>().AddItem();
            GetComponent<ItemController>().DestroyItem(gameObject);
        }
    }
}
