using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject _object;
    public TextMeshProUGUI _textMeshProUGUI;
    private void Update()
    {
        Debug.Log("11");
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("yes");
            _object.SetActive(true);
            ScratchTicket();
        }
    }

    private void ScratchTicket()
    {
        float randomValue = Random.Range(0f, 1f);
        if (randomValue < 0.5f)
        {
            _textMeshProUGUI.text = "success";
            //success
        }
        else
        {
            _textMeshProUGUI.text = "false";
            //lose
        }
    }
}
