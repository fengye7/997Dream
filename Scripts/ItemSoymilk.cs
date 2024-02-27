using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSoymilk : MonoBehaviour
{
    public GameObject Soymilk;
    private void Start()
    {
        Cursor.visible = true;
        DragByInterface dragScript = Soymilk.GetComponent<DragByInterface>();
        dragScript.CountdownSoymilkFinished += OnCountdownSoymilkFinished;
    }
    private void OnCountdownSoymilkFinished()
    {
        Cursor.visible = false;
        GetComponent<ItemController>().AddItem();
        GetComponent<ItemController>().DestroyItem(gameObject);
    }
}
