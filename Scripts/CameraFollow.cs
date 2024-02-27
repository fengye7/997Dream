using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform cam;
    public float rateX, rateY;
    float startPointX, startPointY;
    float cameraStartX, cameraStartY;
    void Start()
    {
        if (!cam)
        {
            cam = Camera.main.transform;
        }
        
        cameraStartX = cam.position.x;
        cameraStartY = cam.position.y;
        startPointX = transform.position.x;
        startPointY = transform.position.y;
    }
    void Update()
    {
        transform.position = new Vector2(startPointX + (cam.position.x - cameraStartX) * rateX, startPointY + (cam.position.y - cameraStartY) * rateY);//按速度比例计算相对位移
    }
}
