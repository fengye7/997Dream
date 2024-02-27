using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FindRabbit : MonoBehaviour, IPointerClickHandler
{
    public delegate void RabbitClickFinishedEventHandler();
    public event RabbitClickFinishedEventHandler RabbitClickFinished;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsPointerOverUI("Rabbit"))
        {
            RabbitClickFinished();
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
                Destroy(result.gameObject);
                return true;
            }
        }
        return false;
    }
}
