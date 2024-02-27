using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 单例实例
    private static CameraController _instance;

    // 是否正在震动
    private bool _isShake;

    // 获取单例实例的静态方法
    public static CameraController Instance
    {
        get
        {
            // 如果_instance为null，则在场景中查找CameraController组件并将其赋值给_instance
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraController>();
                if (_instance == null)
                {
                    Debug.LogError("CameraController Instance not found in scene!");
                }
            }
            return _instance;
        }
    }

    // 摄像机震动方法
    public void CameraShake(float duration, float strength)
    {
        if (!_isShake)
            StartCoroutine(Shake(duration, strength));
    }

    // 震动协程
    IEnumerator Shake(float duration, float strength)
    {
        _isShake = true;

        Transform cameraTransform = Camera.main.transform;
        Vector3 startPosition = cameraTransform.position;

        while (duration > 0)
        {
            cameraTransform.position = Random.insideUnitSphere * strength + startPosition;
            duration -= Time.deltaTime;
            yield return null;
        }

        // 重置摄像机位置
        cameraTransform.position = startPosition;
        _isShake = false;
    }
}

