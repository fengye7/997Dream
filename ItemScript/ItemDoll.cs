using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDoll : MonoBehaviour
{
    public GameObject Rabbit1;
    public GameObject Rabbit2;
    public GameObject Rabbit3;
    private int time;
    bool isCheck = false;
    private void Start()
    {
        Cursor.visible = true;
        time = 0;
        FindRabbit RabbitScript1 = Rabbit1.GetComponent<FindRabbit>();
        FindRabbit RabbitScript2 = Rabbit2.GetComponent<FindRabbit>();
        FindRabbit RabbitScript3 = Rabbit3.GetComponent<FindRabbit>();
        RabbitScript1.RabbitClickFinished += OnRabbitClickFinished;
        RabbitScript2.RabbitClickFinished += OnRabbitClickFinished;
        RabbitScript3.RabbitClickFinished += OnRabbitClickFinished;
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
    private void OnRabbitClickFinished()
    {
        time++;
    }
}
