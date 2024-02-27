using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  记录一个Animator中的参数
/// </summary>
public class PlayerAnimatorHash : MonoBehaviour
{
    public static readonly int IsRunning = Animator.StringToHash("Is Running");
    public static readonly int IsRolling = Animator.StringToHash("Is Rolling");
}
