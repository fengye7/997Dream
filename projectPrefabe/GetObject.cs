using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                GameObject.Find("music/show").gameObject.GetComponent<AudioSource>().Play();
                //Debug.Log("��⵽��Ұ���");
                string gameObjectName = gameObject.name;
                switch(gameObjectName)
                {
                    case "Soymilk":
                        ItemSoymilk ItemSoymilk = gameObject.GetComponent<ItemSoymilk>();
                        ItemSoymilk.enabled = true;
                        break;
                    case "Noodle":
                        ItemNoodle ItemNoodle = gameObject.GetComponent<ItemNoodle>();
                        ItemNoodle.enabled = true;
                        break;
                    case "Barbecue":
                        ItemBarbecue ItemBarbecue = gameObject.GetComponent<ItemBarbecue>();
                        ItemBarbecue.enabled = true;
                        break;
                    case "Popcorn":
                        ItemPopcorn ItemPopcorn = gameObject.GetComponent<ItemPopcorn>();
                        ItemPopcorn.enabled = true;
                        break;
                    case "Pingpong":
                        ItemPingpong ItemPingpong = gameObject.GetComponent<ItemPingpong>();
                        ItemPingpong.enabled = true;
                        break;
                    case "Ferrule":
                        ItemFerrule ItemFerrule = gameObject.GetComponent<ItemFerrule>();
                        ItemFerrule.enabled = true;
                        break;
                    case "Doll":
                        ItemDoll ItemDoll = gameObject.GetComponent<ItemDoll>();
                        ItemDoll.enabled = true;
                        break;
                    case "Cereus":
                        ItemCereus ItemCereus = gameObject.GetComponent<ItemCereus>();
                        //firstday or secondday
                        if (true)
                        {
                            ItemCereus.FirstDay();
                            ItemCereus.enabled = false;
                        }
                        else
                            ItemCereus.SecondDay();
                        break;
                    case "Videogame":
                        ItemVideogame ItemVideogame = gameObject.GetComponent<ItemVideogame>();
                        ItemVideogame.enabled = true;
                        break;
                    case "Lottery":
                        ItemLottery ItemLottery = gameObject.GetComponent<ItemLottery>();
                        ItemLottery.enabled = true;
                        break;
                    case "Runningshoes":
                        ItemRunningshoes ItemRunningshoes = gameObject.GetComponent<ItemRunningshoes>();
                        ItemRunningshoes.enabled = true;
                        ItemRunningshoes.SetinitialPosition(gameObject.transform.position);
                        break;
                    case "Amulet":
                        ItemAmulet ItemAmulet = gameObject.GetComponent<ItemAmulet>();
                        ItemAmulet.CollectFragment(gameObject.name);
                        break;
                }
            }
        }
    }
}
