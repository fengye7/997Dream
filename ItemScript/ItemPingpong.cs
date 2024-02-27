using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPingpong : MonoBehaviour
{
    public GameObject Racket;
    public GameObject Bottom;
    private void Start()
    {
        Racket RacketFinish = Racket.GetComponent<Racket>();
        RacketFinish.PingpongSuccess += OnPingpongSuccess;
        Bottom BottomFinish = Bottom.GetComponent<Bottom>();
        BottomFinish.PingpongFailed += OnPingpongFailed;
    }
    private void OnPingpongSuccess()
    {
        GetComponent<ItemController>().AddItem();
        GetComponent<ItemController>().DestroyItem(gameObject);
    }
    private void OnPingpongFailed()
    {
        GetComponent<ItemController>().DestroyItem(gameObject);
    }
}
