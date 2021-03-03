using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable] public class UnityEventFloat : UnityEvent<float> { }

public class InputManager : MonoSingleton, INewInputSystem
{
    [SerializeField] InputAction _moveAction, _runAction, _jumpAction, _shootAction;
    [SerializeField] UnityEventFloat _OnMovePerformed, _OnRunPerformed, _OnJumpPerformed, _OnShootPerformed;

    private void OnEnable() => Enable();
    private void OnDisable() => Disable();
    private void Awake()
    {
        EventHandlers();
        base.MakeSingleton();
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
        _moveAction.performed += ctx => LeftAndRightInputActionPerformed(ctx.ReadValue<float>());
        _moveAction.canceled += ctx => LeftAndRightInputActionPerformed(ctx.ReadValue<float>());

        _runAction.performed += ctx => RunInputActionPerformed(ctx.ReadValue<float>());
        _runAction.canceled += ctx => RunInputActionPerformed(ctx.ReadValue<float>());

        _jumpAction.performed += ctx => JumpInputActionPerformed(ctx.ReadValue<float>());
        _jumpAction.canceled += ctx => JumpInputActionPerformed(ctx.ReadValue<float>());

        _shootAction.performed += ctx => OnShootPerformed(ctx.ReadValue<float>());
    }

    public void LeftAndRightInputActionPerformed(float ctx) => _OnMovePerformed?.Invoke(ctx);
    public void RunInputActionPerformed(float ctx) => _OnRunPerformed?.Invoke(ctx);
    public void JumpInputActionPerformed(float ctx) => _OnJumpPerformed?.Invoke(ctx);
    public void OnShootPerformed(float ctx) => _OnShootPerformed?.Invoke(ctx);
}
