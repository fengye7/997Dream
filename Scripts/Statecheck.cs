using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statecheck : MonoBehaviour
{
    // Start is called before the first frame update
   // public Glowing glowing;
   // public UIcontroller controller;
    public bool _state;
    public Sprite _openSprite;
    public Sprite _closeSprite;
    public bool isOnce, isUse;

    private void Update()
    {
        objectState(_state);
    }
    public void objectState(bool state)
    {
        if(state && !isUse)
        {
             gameObject.GetComponent<Glowing>().enabled = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = _openSprite;
            gameObject.GetComponent<UIcontroller>().enabled = true;
            
        }
        else
        {
            gameObject.GetComponent<Glowing>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = _closeSprite;
            gameObject.GetComponent<UIcontroller>().enabled = false;
            gameObject.GetComponent<UIcontroller>().UI.SetActive(false);
        }
    }
}
