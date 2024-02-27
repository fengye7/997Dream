using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

/// <summary>
/// 角色的相关可控数值
/// </summary>
[Serializable]
public class PlayerParamater
{
    public Transform _playerTransform;
    public Rigidbody2D _rigidbody2D;
    public Animator _animator;

    public Vector3 _moveDir;

    [Header("Attack Info")]
    public float _playerSpeed = 10f;
    public float _rollSpeed = 20f;
    public float _playerDamage = 5;

    public PlayerSkills _skills;
}

/// <summary>
/// 角色状态机
/// </summary>
public class PlayerFSM : MonoBehaviour
{
    public PlayerParamater _paramater; 
    public PlayerIState _currentState;
    public Dictionary<StateType, PlayerIState> _states = new Dictionary<StateType, PlayerIState>();


    private void Awake()
    {
        _paramater._playerTransform = GetComponent<Transform>();
        _paramater._rigidbody2D = GetComponent<Rigidbody2D>();
        _paramater._animator = GetComponent<Animator>();
        _paramater._skills = FindObjectOfType<PlayerSkills>();
    }

    void Start()
    {
        _states.Add(StateType.PlayerIdle, new PlayerIdleState(this));
        _states.Add(StateType.PlayerRoll, new PlayerRollState(this));
        _states.Add(StateType.PlayerRun, new PlayerRunState(this));

        TransitionState(StateType.PlayerIdle);
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        ControllPlayerFacing();
    }

    public void TransitionState(StateType state)
    {
        if(_currentState != null)
        {
            _currentState.OnExit();
        }
        _currentState = _states[state];
        _currentState.OnEnter();
    }

    /// <summary>
    /// 切换状态时播放对应的动画
    /// </summary>
    /// <param name="animation"></param>
    public void PlayAnimation(string animation)
    {
        _paramater._animator.Play(animation);
    }

    /// <summary>
    /// 控制人物朝向
    /// </summary>
    private void ControllPlayerFacing()
    {
        float turnX = Input.GetAxisRaw("Horizontal");
        if (turnX > 0)
            transform.localScale = new Vector3(-1, 1, 1); //翻转
        else if (turnX < 0)
            transform.localScale = Vector3.one; //Vector3.one就是Vector3(1,1,1)

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1); //翻转
        else transform.localScale = Vector3.one;
    }
}


/// <summary>
/// 角色相关动画的名称声明
/// </summary>
public static class PlayerAnimationName
{
    public const string PlayerIdle = "PlayerIdle";
    public const string PlayerRoll = "PlayerRoll";
    public const string PlayerRun = "PlayerRun";
}

/// <summary>
/// 状态机的状态枚举
/// </summary>
public enum StateType
{
    PlayerIdle,
    PlayerRoll,
    PlayerRun,
}