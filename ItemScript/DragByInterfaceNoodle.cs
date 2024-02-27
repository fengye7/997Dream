using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragByInterfaceNoodle : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTrans;
    private CanvasGroup canvasGroup;
    private Vector2 initialPosition;
    public delegate void DragFinishedEventHandler();
    public event DragFinishedEventHandler DragFinished;

    private void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rectTrans.anchoredPosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.5f;
        Vector2 dragStartPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        rectTrans.position = worldMousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if (!IsPointerOverUI("Noodle"))
        {
            rectTrans.anchoredPosition = initialPosition;
        }
        else
        {
            Destroy(draggedObject);
            DragFinished();
        }
    }
    bool IsPointerOverUI(string uiElementName)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var result in results)
        {
            if (result.gameObject.name == uiElementName)
            {
                return true;
            }
        }
        return false;
    }
}