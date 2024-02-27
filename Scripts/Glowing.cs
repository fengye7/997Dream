using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Glowing : MonoBehaviour
{
    // Start is called before the first frame update
    public Material _newMaterial;
    private Material _oldMaterial;
    private int _mat = 0;
  //  public TextMeshProUGUI _textMeshProUGUI;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        _oldMaterial = renderer.material;
       //������������������ַ���;
       //����
       //GetString("JKKLJ");
    }

    // Update is called once per frame
    void Update()
    {
        if (_mat == 0)
        {
            Matrialold();
        }
        else
        {
            Matreailnew();
        }

      //  if(Input.GetKeyDown(KeyCode.E)) { GetString("JKLJLLKJ"); }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        _mat = 1;
     //   _textMeshProUGUI.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _mat = 0;
       // _textMeshProUGUI.gameObject.SetActive(false);

    }
    private void Matrialold()
    {
        Renderer renderer = GetComponent<Renderer>();
        GetComponent<Renderer>().material = _oldMaterial;
    }
    private void Matreailnew()
    {
        Renderer renderer = GetComponent<Renderer>();
        GetComponent<Renderer>().material = _newMaterial;
    }
    /*private void GetString(string tip)//������������������ַ���
    {
        // tip = "JKLJKL";
        _textMeshProUGUI.text = tip;
    }*/
}
