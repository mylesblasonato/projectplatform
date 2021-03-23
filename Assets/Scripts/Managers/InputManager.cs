using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable] public class UnityEventFloat : UnityEvent<float> { }
[System.Serializable] public class UnityEventBool : UnityEvent<bool> { }

public class InputManager : MonoBehaviour, INewInputSystem
{
    public string _horAxis, _verAxis, _walkAxis, _jumpAxis, _shootAxis, _climbAxis;
    public UnityEventFloat _OnMovePerformed, _OnWalkPerformed, _OnJumpPerformed, _OnCrouchPerformed, _OnShootPerformed, _OnClimbPerformed;

    void Update()
    {
        if (GameManager.Instance.IsPaused) return;
        if (Mathf.Abs(Input.GetAxis(_horAxis)) >= 0)
            _OnMovePerformed?.Invoke(Input.GetAxis(_horAxis));
        if (Input.GetAxis(_verAxis) < 0)
            _OnCrouchPerformed?.Invoke(Input.GetAxis(_verAxis));
        if (Mathf.Abs(Input.GetAxis(_walkAxis)) >= 0)
            _OnWalkPerformed?.Invoke(Input.GetAxis(_walkAxis));
        if (Input.GetButtonDown(_jumpAxis))
            _OnJumpPerformed?.Invoke(Input.GetAxis(_jumpAxis));
        if (Input.GetButtonDown(_shootAxis))
            _OnShootPerformed?.Invoke(Input.GetAxis(_shootAxis));
        if (Input.GetAxis(_climbAxis) > 0)
            _OnClimbPerformed?.Invoke(Input.GetAxis(_climbAxis));
    }

    public void MovePerformed(InputAction.CallbackContext ctx) 
    { 
        if (GameManager.Instance.IsPaused) return;
        _OnMovePerformed?.Invoke(ctx.ReadValue<Vector2>().x);
    
    }
    public void WalkPerformed(InputAction.CallbackContext ctx) 
    { 
        if (GameManager.Instance.IsPaused) return; 
        _OnWalkPerformed?.Invoke(ctx.ReadValue<float>()); 
    
    }
    public void JumpPerformed(InputAction.CallbackContext ctx) 
    { 
        if (GameManager.Instance.IsPaused) return; 
        _OnJumpPerformed?.Invoke(ctx.ReadValue<float>()); 
    }

    public void CrouchPerformed(InputAction.CallbackContext ctx) 
    { 
        if (GameManager.Instance.IsPaused) return; 
        _OnCrouchPerformed?.Invoke(ctx.ReadValue<float>()); 
    }

    public void ShootPerformed(InputAction.CallbackContext ctx) 
    { 
        if (GameManager.Instance.IsPaused) return; 
        _OnShootPerformed?.Invoke(ctx.ReadValue<float>()); 
    }

    public void ClimbPerformed(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.IsPaused) return;
        _OnClimbPerformed?.Invoke(ctx.ReadValue<float>());
    }

    // NOT USED
    public void EventHandlers()
    {
        throw new NotImplementedException();
    }

    public void Enable()
    {
        throw new NotImplementedException();
    }

    public void Disable()
    {
        throw new NotImplementedException();
    }
}
