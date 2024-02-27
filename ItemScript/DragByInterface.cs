using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragByInterface : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTrans;
    private CanvasGroup canvasGroup;
    private Vector2 initialPosition;
    [SerializeField] private float destructionDelay;
    public delegate void CountdownSoymilkFinishedEventHandler();
    public event CountdownSoymilkFinishedEventHandler CountdownSoymilkFinished;

    private void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rectTrans.anchoredPosition;
        destructionDelay = 4f;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.5f;
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
        if (!IsPointerOverUI("Fermenter"))
        {
            rectTrans.anchoredPosition = initialPosition;
        }
        else
        {
            Cursor.visible = false;
            StartCoroutine(DestroyAfterDelay(draggedObject, destructionDelay));
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
    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        float elapsedTime = 0f;
        while (elapsedTime < delay)
        {
            string time = Mathf.CeilToInt(delay - elapsedTime).ToString();
            Transform timeClockTransform = transform.parent.Find("TimeClock");
            if (timeClockTransform != null)
            {
                Transform timeTransform = timeClockTransform.Find("Time");
                if (timeTransform != null)
                {
                    Text timeText = timeTransform.GetComponent<Text>();
                    timeText.text = time;
                }
                else
                {
                    Debug.LogError("Time transform not found under TimeClock.");
                }
            }
            else
            {
                Debug.LogError("TimeClock transform not found under ItemSoymilk.");
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
        CountdownSoymilkFinished();
    }
}
