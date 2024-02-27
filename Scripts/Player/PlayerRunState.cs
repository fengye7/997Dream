using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerIState
{
    public PlayerFSM _fsm;
    public PlayerParamater _paramater;

    public PlayerRunState(PlayerFSM fsm)
    {
        _fsm = fsm;
        _paramater = fsm._paramater;
    }

    public void OnEnter()
    {
        _paramater._animator.SetBool(PlayerAnimatorHash.IsRunning, true);
        _fsm.PlayAnimation(PlayerAnimationName.PlayerRun);
    }

    public void OnUpdate()
    {
        float runValueX = Input.GetAxisRaw("Horizontal");
        float runValueY = Input.GetAxisRaw("Vertical");
        _paramater._moveDir = new Vector3(runValueX, runValueY).normalized;
        _paramater._rigidbody2D.velocity = _paramater._moveDir * _paramater._playerSpeed;

        if(runValueX ==0 && runValueY == 0)
        {
            _fsm.TransitionState(StateType.PlayerIdle);
        }

        // ����Ƿ��Ѿ�������ȴʱ�䣬���Ұ�����Space��
        if (Time.time - _paramater._skills._lastRollTime >= _paramater._skills._rollCooldown && Input.GetKeyDown(KeyCode.Space))
        {
            _fsm.TransitionState(StateType.PlayerRoll);
            _paramater._skills._lastRollTime = Time.time; // ������һ��ʹ��Roll���ܵ�ʱ��
            _paramater._skills.StartRollCooldown();
        }
    }

    public void OnExit()
    {
        _paramater._animator.SetBool(PlayerAnimatorHash.IsRunning, false);
    }
}