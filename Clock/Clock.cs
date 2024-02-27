using UnityEngine;

public class Clock : MonoBehaviour
{
    public float startTime, endTime, duration;
    Transform hour, minute;
    public bool isOver = false;
    void Start()
    {
        hour = transform.GetChild(0).transform;
        minute = transform.GetChild(1).transform;
        hour.transform.Rotate(0, 0, 30 * (12 - startTime));
        // �ڿ�ʼʱ���� OverTime ����
        Invoke("OverTime", duration);
    }


    void Update()
    {
        
        if(endTime > startTime)
        {
            hour.transform.Rotate(0, 0, (-(endTime - startTime) * 30 * Time.deltaTime) / duration);
            minute.transform.Rotate(0, 0,(-360 * (endTime - startTime)* Time.deltaTime) /duration);
        }
        else
        {
            hour.transform.Rotate(0, 0, (-(endTime + 12 - startTime) * 30 * Time.deltaTime) / duration);
            minute.transform.Rotate(0, 0, (-360 * (endTime + 12 - startTime) * Time.deltaTime) / duration);
        }
    }
    public void OverTime()
    {
        Debug.Log("over");
        isOver = true;
        Utils.GameOver("ʱ�䵽���ξ�����������");
    }
}
