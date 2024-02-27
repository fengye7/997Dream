using UnityEngine;


//public enum _isover
//{
//    no,
//    yes
//}

public class Pickup : MonoBehaviour
{
    //private Inventory inventory;
    public GameObject itemButton;
    //public _isover enventIsover = _isover.no;
    //public Sprite itemSprite;

    private void Start()
    {
        //inventory=GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

    }
    private void Update()
    {
        //Selected();
        if (transform.GetChild(0).gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.E))
        {
            if (GetComponent<Statecheck>().isOnce)
            {
                GetComponent<Statecheck>().isUse = true;
            }
            Instantiate(itemButton, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);

        }
        //private void Selected()
        //{
        //if(enventIsover==_isover.yes)
        //{
        //    Pick();
        //}
        //else
        //{
        //    //Debug.Log("Not over");
        //}
        // }
        //private void Pick()//pick up the object
        //{

        //for (int i = 0;i<inventory.slots.Length;i++)
        //{
        //    if (inventory.isFull[i]==false) 
        //    { 
        //        inventory.isFull[i] = true;
        //        Instantiate(itemButton, inventory.slots[i].);
        //       // Destroy(gameObject);
        //        break;                                
        //    }
        //}

        //}
        //private void OnTriggerStay2D(Collider2D collision)
        //{
        //    if(collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        //    {
        //        Instantiate(itemButton, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        //    }
        //}

    }
}
