using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ItemBarbecue : MonoBehaviour
{
    private string targetString;
    private char playerInput;
    private int length;
    public delegate void BarbecueFinishedEventHandler(bool output);
    public event BarbecueFinishedEventHandler BarbecueFinished;

    void Start()
    {
        GenerateTargetString();
        length = -1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            playerInput = 'J';
            Transform sugarTransform = transform.Find("Canvas/PanelBarbecue/Sugar");
            StartCoroutine(ScaleChange(sugarTransform));
            CheckInput();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            playerInput = 'K';
            Transform paprikaTransform = transform.Find("Canvas/PanelBarbecue/Paprika");
            StartCoroutine(ScaleChange(paprikaTransform));
            CheckInput();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            playerInput = 'L';
            Transform cuminumTransform = transform.Find("Canvas/PanelBarbecue/Cuminum");
            StartCoroutine(ScaleChange(cuminumTransform));
            CheckInput();
        }
    }

    void GenerateTargetString()
    {
        targetString = "";
        System.Random random = new System.Random();
        for (int i = 0; i < 7; i++)
        {
            int randomIndex = random.Next(3);
            char randomChar = (char)('J' + randomIndex);
            targetString += randomChar;
        }
        string orderstring = "";
        for (int i = 0; i < 7; i++) 
        {
            if (targetString[i] == 'J')
                orderstring += "ÌÇ ";
            else if (targetString[i] == 'K')
                orderstring += "À±½· ";
            else if (targetString[i] == 'L')
                orderstring += "×ÎÈ» ";
        }
        Transform order = transform.Find("Canvas/PanelBarbecue/Order");
        if (order != null)
        {
            Text orderText = order.GetComponent<Text>();
            orderText.text = orderstring;
        }
    }

    void CheckInput()
    {
        length++;
        if (playerInput != targetString[length])
        {
            length = -1;
            GetComponent<ItemController>().DestroyItem(gameObject);
        }
        else if (length >= 6)
        {
            GetComponent<ItemController>().AddItem();
            GetComponent<ItemController>().DestroyItem(gameObject);
        }
    }

    IEnumerator ScaleChange(Transform image)
    {
        Vector3 imagescale = image.transform.localScale;
        image.transform.DOScale(0.9f * imagescale, 0.15f);
        yield return new WaitForSeconds(0.15f);
        image.transform.DOScale(imagescale, 0.15f);
    }
}
