using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FuPart : MonoBehaviour
{
    
    public GameObject fu;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            fu.GetComponent<ItemHushenfu>().fuNum++;
            transform.DOScale(0, 1f);
            Destroy(gameObject, 1f);
        }
    }

}
