using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable] public class UnityEventFloat : UnityEvent<float> { }
[System.Serializable] public class UnityEventBool : UnityEvent<bool> { }

public class InputManager : MonoBehaviour, INewInputSystem
{
    public InputAction _moveAction, _runAction, _jumpAction, _shootAction;
    public UnityEventFloat _OnMovePerformed;
    public UnityEventFloat _OnRunPerformed, _OnJumpPerformed, _OnShootPerformed;

    private void OnEnable() => Enable();
    private void OnDisable() => Disable();
    private void Awake()
    {
        EventHandlers();
    }
    
    public void Enable()
    {
        _moveAction.Enable();
        _runAction.Enable();
        _jumpAction.Enable();
        _shootAction.Enable();
    }

    public void Disable()
    {
        _moveAction.Disable();
        _runAction.Disable();
        _jumpAction.Disable();
        _shootAction.Disable();
    }

    public void EventHandlers()
    {
        _moveAction.performed += ctx => MovePerformed(ctx);
        _moveAction.canceled += ctx => MovePerformed(ctx);

        _runAction.performed += ctx => RunInputActionPerformed(ctx);
        _runAction.canceled += ctx => RunInputActionPerformed(ctx);

        _jumpAction.performed += ctx => JumpInputActionPerformed(ctx);
        _jumpAction.canceled += ctx => JumpInputActionPerformed(ctx);

        _shootAction.performed += ctx => OnShootPerformed(ctx);
    }

    public void MovePerformed(InputAction.CallbackContext ctx) => _OnMovePerformed?.Invoke(ctx.ReadValue<Vector2>().x);
    public void RunInputActionPerformed(InputAction.CallbackContext ctx) => _OnRunPerformed?.Invoke(ctx.ReadValue<float>());
    public void JumpInputActionPerformed(InputAction.CallbackContext ctx) => _OnJumpPerformed?.Invoke(ctx.ReadValue<float>());
    public void OnShootPerformed(InputAction.CallbackContext ctx) => _OnShootPerformed?.Invoke(ctx.ReadValue<float>());
}
