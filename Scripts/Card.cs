using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update
  public void OnClick()
    {
        HandManager.instance.AddShoe();
        Destroy(gameObject);
    }
}
