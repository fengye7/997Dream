using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色动画机中的事件触发
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
