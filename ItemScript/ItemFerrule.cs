using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFerrule : MonoBehaviour
{
    public GameObject CircleRed;
    public GameObject CircleGreen;
    public GameObject CircleBlue;
    private bool red = false;
    private bool blue = false;
    private bool green = false;
    bool isCheck = false;
    private void Start()
    {
        Cursor.visible = true;
        DragRedCircle circleRed = CircleRed.GetComponent<DragRedCircle>();
        DragGreenCircle circleGreen = CircleGreen.GetComponent<DragGreenCircle>();
        DragBlueCircle circleBlue = CircleBlue.GetComponent<DragBlueCircle>();
        circleRed.DragRed += OnDragRed;
        circleGreen.DragGreen += OnDragGreen;
        circleBlue.DragBlue += OnDragBlue;
    }
    private void Update()
    {
        if (red == true && blue == true && green == true && !isCheck)
        {
            isCheck = true;
            Cursor.visible = false;
            GetComponent<ItemController>().AddItem();
            GetComponent<ItemController>().DestroyItem(gameObject);
        }
    }
    private void OnDragRed()
    {
        red = true;
    }
    private void OnDragGreen()
    {
        green = true;
    }
    private void OnDragBlue()
    {
        blue = true;
    }
}
