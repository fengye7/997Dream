using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopcorn : MonoBehaviour
{
    private char targetLetter;
    private int successfulAttempts;
    private bool Isstart;
    private bool pushkey;
    KeyCode Letter;
    private Coroutine timerCoroutine;

    void Start()
    {
        Isstart = false;
        successfulAttempts = 0;
        GenerateRandomLetter();
    }

    void Update()
    {
        if (Isstart)
        {
            CheckPlayerInput();
        }
        else
        {
            if (Input.GetKeyDown(Letter))
            {
                Isstart = true;
                Transform keyTransform = transform.Find("CanvasPopcorn/PanelPopcorn/Key");
                Transform keyTextTransform = keyTransform.Find("KeyText");
                Text keyText = keyTextTransform.GetComponent<Text>();
                keyText.text = "";
                StartCoroutine(ScaleChange(keyTransform));
                Invoke("StartGame", 2f);
            }
        }
    }

    void StartGame()
    {
        InvokeRepeating("GenerateRandomLetter", 0f, 2f);
    }

    void GenerateRandomLetter()
    {
        Random.InitState((int)Time.time);
        targetLetter = (char)Random.Range('J', 'L' + 1);
        Letter = ConvertCharToKeyCode(targetLetter);
        Transform keyTextTransform = transform.Find("CanvasPopcorn/PanelPopcorn/Key/KeyText");
        if (keyTextTransform != null)
        {
            Text keyText = keyTextTransform.GetComponent<Text>();
            keyText.text = targetLetter.ToString();
        }
        if (Isstart)
        {
            pushkey = false;
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }
    }

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(1f);
        if (!pushkey)
        {
            CancelInvoke("GenerateRandomLetter");
            GetComponent<ItemController>().DestroyItem(gameObject);
        }
    }

    void CheckPlayerInput()
    {
        Transform keyTransform = transform.Find("CanvasPopcorn/PanelPopcorn/Key");
        if (Input.GetKeyDown(Letter))
        {
            Transform keyTextTransform = keyTransform.Find("KeyText");
            Text keyText = keyTextTransform.GetComponent<Text>();
            keyText.text = "";
            pushkey = true;
            StartCoroutine(ScaleChange(keyTransform));
            successfulAttempts++;
            if (successfulAttempts >= 4)
            {
                CancelInvoke("GenerateRandomLetter");
                GetComponent<ItemController>().AddItem();
                GetComponent<ItemController>().DestroyItem(gameObject);
            }
        }
        else if (Input.anyKeyDown)
        {
            CancelInvoke("GenerateRandomLetter");
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

    KeyCode ConvertCharToKeyCode(char character)
    {
        switch (character)
        {
            case 'J': return KeyCode.J;
            case 'K': return KeyCode.K;
            case 'L': return KeyCode.L;
            default: return KeyCode.None;
        }
    }
}
