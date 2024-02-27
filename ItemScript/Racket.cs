using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class Racket : MonoBehaviour
{
    private int collidetime;
    private float movementSpeed = 10f;
    public delegate void PingpongSuccessEventHandler();
    public event PingpongSuccessEventHandler PingpongSuccess;
    bool isend = false;
    private void Start()
    {
        collidetime = 0;
    }
    private void Update()
    {
        RacketMove();
        if(collidetime >= 5 && !isend)
        {
            isend = true;
            PingpongSuccess();
        }
    }
    public void RacketMove()
    {
        float horizontalInput =Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(horizontalInput, 0, 0) * Time.deltaTime * movementSpeed);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.relativeVelocity.y <= 0f)
        {
            collidetime++;
            GameObject collideObject = collision.collider.gameObject;
            if(collideObject.name == "Pingpong")
            {
                Rigidbody2D pingpong = collideObject.GetComponent<Rigidbody2D>();
                if(pingpong != null)
                {
                    float upwardForce = Random.Range(5f,8f);
                    float horizontalOffset = Random.Range(-1f,1f);
                    pingpong.AddForce(new Vector2(horizontalOffset, upwardForce), ForceMode2D.Impulse);
                    StartCoroutine(StopUpwardMotionAfterDelay(pingpong));
                }
            }
        }
    }
    private IEnumerator StopUpwardMotionAfterDelay(Rigidbody2D pingpong)
    {
        yield return new WaitForSeconds(1f);
        pingpong.velocity = new Vector2(pingpong.velocity.x, pingpong.velocity.y);
    }
}
