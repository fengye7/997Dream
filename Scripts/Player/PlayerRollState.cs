using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerIState
{
    public PlayerFSM _fsm;
    public PlayerParamater _paramater;

    public PlayerRollState(PlayerFSM fsm)
    {
        _fsm = fsm;
        _paramater = fsm._paramater;
    }

    public void OnEnter()
    {
        _paramater._animator.SetBool(PlayerAnimatorHash.IsRolling, true);
        _fsm.PlayAnimation(PlayerAnimationName.PlayerRoll);
        DreamSceneAudios.Instance.PlayRollAudio();
    }

    public void OnUpdate()
    {
        _paramater._rigidbody2D.velocity = _paramater._moveDir * _paramater._rollSpeed;
        if (_paramater._animator.GetBool(PlayerAnimatorHash.IsRolling) == false)
        {
            _paramater._rigidbody2D.velocity = Vector2.zero;
            _fsm.TransitionState(StateType.PlayerIdle);
        }
    }

    public void OnExit()
    {

    }
}
