using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemController : MonoBehaviour
{
    public GameObject itemSprite;
    string itemName;
    void Start()
    {
        itemName = itemSprite.name;
    }

    
    void Update()
    {
        
    }
    public void AddItem()
    {
        GameObject slot = GameObject.Find("Canvas1/Slot").gameObject;
        for (int i = 0; i < slot.transform.childCount; i++)
        {
            if(slot.transform.GetChild(i).gameObject.transform.childCount < 1)
            {
                GameObject item = Instantiate(itemSprite, Vector3.zero, Quaternion.identity, slot.transform.GetChild(i).gameObject.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.DOScale(0f,0.8f).From();
                RealityData.Instance._itemList.Add(itemName);
                break;
            }
        }
    }
    public void DestroyItem(GameObject obj)
    {
        StartCoroutine(ScaleChange(obj));
    }
    public IEnumerator ScaleChange(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.transform.DOScale(0, 0.8f);
        yield return new WaitForSeconds(0.8f);
        Destroy(obj);
    }
}
