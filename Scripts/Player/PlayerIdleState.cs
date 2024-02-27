using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerIdleState : PlayerIState
{
    public PlayerFSM _fsm;
    public PlayerParamater _paramater;

    public PlayerIdleState(PlayerFSM fsm)
    {
        _fsm = fsm;
        _paramater = fsm._paramater;
    }

    public void OnEnter()
    {
        _fsm.PlayAnimation(PlayerAnimationName.PlayerIdle);
    }

    public void OnUpdate()
    {
        float runValueX = Input.GetAxisRaw("Horizontal");
        float runValueY = Input.GetAxisRaw("Vertical");
        _paramater._moveDir = new Vector3(runValueX, runValueY).normalized;
        if(runValueX != 0 || runValueY != 0)
        {
            _fsm.TransitionState(StateType.PlayerRun);
        }
    }
    public void OnExit()
    {

    }
}
