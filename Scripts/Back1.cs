using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back1 : MonoBehaviour
{
    public float speed = 0.0003f;
    Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
    }


    void Update()
    {
        transform.position = startPos + new Vector3(Input.mousePosition.x * speed, Input.mousePosition.y * speed, 0);
    }
}
