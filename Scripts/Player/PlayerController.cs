using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// ��ҽ�ɫ��������
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _playerSpeed = 10f;

    private Vector2 _moveDirection;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _moveDirection *= _playerSpeed;

        //_animator.SetFloat(PlayerAnimatorHash.MoveSpeed, Mathf.Abs(_moveDirection.x)+Mathf.Abs(_moveDirection.y));
    }

    private void FixedUpdate()
    {
        _rigidbody2D.AddForce(_moveDirection, ForceMode2D.Impulse);
        ControllPlayerFacing();
    }

    private void ControllPlayerFacing()
    {
        float turnX = Input.GetAxis("Horizontal");
        if (turnX > 0)
            transform.localScale = new Vector3(-1, 1, 1); //��ת������ֵΪ��
        else if (turnX < 0)
            transform.localScale = Vector3.one; //Vector3.one��Vector3(1,1,1)
    }
}
