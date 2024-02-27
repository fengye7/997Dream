using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ�������е��¼�����
/// </summary>
public class PlayerAnimEvents : MonoBehaviour
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void FinishRoll()
    {
        _animator.SetBool(PlayerAnimatorHash.IsRolling, false);
    }
}
