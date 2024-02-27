using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottom : MonoBehaviour
{
    public delegate void PingpongFailedEventHandler();
    public event PingpongFailedEventHandler PingpongFailed;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.relativeVelocity.y <= 0f)
        {
            GameObject collideObject = collision.collider.gameObject;
            if(collideObject.name == "Pingpong")
            {
                Rigidbody2D pingpong = collideObject.GetComponent<Rigidbody2D>();
                if(pingpong != null)
                {
                    PingpongFailed();
                }
            }
        }
    }
}
