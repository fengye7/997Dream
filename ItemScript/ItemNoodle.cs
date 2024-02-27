using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNoodle : MonoBehaviour
{
    public GameObject Bamboo;
    public GameObject Peanut;
    public GameObject Yuba;
    private int time;
    bool isCheck = false;
    private void Start()
    {
        time = 0;
        Cursor.visible = true;
        DragByInterfaceNoodle dragBambooScript = Bamboo.GetComponent<DragByInterfaceNoodle>();
        DragByInterfaceNoodle dragPeanutScript = Peanut.GetComponent<DragByInterfaceNoodle>();
        DragByInterfaceNoodle dragYubaScript = Yuba.GetComponent<DragByInterfaceNoodle>();
        dragBambooScript.DragFinished += OnDragFinished;
        dragPeanutScript.DragFinished += OnDragFinished;
        dragYubaScript.DragFinished += OnDragFinished;
    }
    private void Update()
    {
        if (time >= 3 && !isCheck)
        {
            isCheck = true;
            Cursor.visible = false;
            GetComponent<ItemController>().AddItem();
            GetComponent<ItemController>().DestroyItem(gameObject);
        }
    }
    private void OnDragFinished()
    {
        time++;
    }
}
