using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContoller2D : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D _rgb;
    private Animator _animator;
    Vector3 _flippedScale = new Vector3(-1, 1, 1);
    public float _moveX;
    public float _moveSpeed=10f;
    public int moveChangeAni;
    public float _startPosX, _endPosX;

    private static PlayerContoller2D instance;
    public static PlayerContoller2D Instance
    {
        get
        {
            if (instance == null)
                instance = Transform.FindObjectOfType<PlayerContoller2D>();
            return instance;
        }
    }
    void Start()
    {
        _rgb =GetComponent<Rigidbody2D>();
        _animator=GetComponent<Animator>();

    }


   
    private void Update()
    {
        //玩家进度条
        GameObject.Find("Canvas/progress").gameObject.GetComponent<Slider>().value = (transform.position.x - _startPosX) / (_endPosX - _startPosX);

    }
    void FixedUpdate()
    {
        Movement();
        Direction();
    }
    private void Movement()
    {
        _moveX = Input.GetAxisRaw("Horizontal");

        _rgb.velocity = new Vector2(_moveX * _moveSpeed, _rgb.velocity.y);
        if(_moveX>0)
        {
            moveChangeAni = 1;
        }
        else if(_moveX<0)
        {
            moveChangeAni=-1;
        }
          else { moveChangeAni = 0; }
        _animator.SetInteger("movement", moveChangeAni);
    }
    private void Direction()
    {
        if (_moveX >0)
        {
            transform.localScale = _flippedScale;
        }
        else if (_moveX <0)
        {
            transform.localScale = Vector3.one;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "door")
        {
            GameObject.Find("music/door").gameObject.GetComponent<AudioSource>().Play();
            GetComponent<Scene>().Load();
        }
    }
}
