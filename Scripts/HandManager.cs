using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    public GameObject _shoe1;
    public GameObject _shoe2;
    private GameObject currentshoe1;
    private GameObject currentshoe2;
    public TextMeshProUGUI _shoeText;
    private int x=0;
    public static HandManager instance { get; private set; }
    private void Awake()
    {
        _shoeText.text = "Pairing By Color";
        instance = this; 
    }


        public void AddShoe()
    {
        if (x == 0)
            currentshoe1 = Instantiate(_shoe1);
        if (x == 1)
            currentshoe2 = Instantiate(_shoe2);

    }
   public void OnCellClick(Cell cell)
    {
        if (x == 0)
            currentshoe1.transform.position = cell.transform.position;

        
        if (x == 1)
        { 
            currentshoe2.transform.position = cell.transform.position;
          
        }
        x++;
        if (x == 2)
        {
         
            _shoeText.text = "you win";
            GetComponent<ItemController>().AddItem();
            GetComponent<ItemController>().DestroyItem(gameObject);
            currentshoe1.transform.DOScale(0, 1.8f);
            currentshoe2.transform.DOScale(0, 1.8f);
        }
     
    }


}
