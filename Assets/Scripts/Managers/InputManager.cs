using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable] public class UnityEventFloat : UnityEvent<float> { }
[System.Serializable] public class UnityEventBool : UnityEvent<bool> { }

public class InputManager : MonoBehaviour, INewInputSystem
{
    public InputAction _moveAction, _runAction, _jumpAction, _crouchAction, _shootAction;
    public UnityEventFloat _OnMovePerformed, _OnRunPerformed, _OnJumpPerformed, _OnCrouchPerformed, _OnShootPerformed;

    private void OnEnable() => Enable();
    private void OnDisable() => Disable();
    private void Start()
    {
        EventHandlers();
    }
    
    public void Enable()
    {
        _moveAction.Enable();
        _runAction.Enable();
        _jumpAction.Enable();
        _crouchAction.Enable();
        _shootAction.Enable();
    }

    public void Disable()
    {
        _moveAction.Disable();
        _runAction.Disable();
        _jumpAction.Disable();
        _crouchAction.Disable();
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

        _crouchAction.performed += ctx => CrouchInputActionPerformed(ctx);
        //_crouchAction.canceled += ctx => CrouchInputActionPerformed(ctx);

        _shootAction.performed += ctx => OnShootPerformed(ctx);
    }

    public void MovePerformed(InputAction.CallbackContext ctx) { if (GameManager.Instance.GetComponent<GameManager>().IsPaused) return; _OnMovePerformed?.Invoke(ctx.ReadValue<Vector2>().x); }
    public void RunInputActionPerformed(InputAction.CallbackContext ctx) { if (GameManager.Instance.GetComponent<GameManager>().IsPaused) return; _OnRunPerformed?.Invoke(ctx.ReadValue<float>()); }
    public void JumpInputActionPerformed(InputAction.CallbackContext ctx) { if (GameManager.Instance.GetComponent<GameManager>().IsPaused) return; _OnJumpPerformed?.Invoke(ctx.ReadValue<float>()); }
    public void CrouchInputActionPerformed(InputAction.CallbackContext ctx) { if (GameManager.Instance.GetComponent<GameManager>().IsPaused) return; _OnCrouchPerformed?.Invoke(ctx.ReadValue<float>()); }
    public void OnShootPerformed(InputAction.CallbackContext ctx) { if (GameManager.Instance.GetComponent<GameManager>().IsPaused) return; _OnShootPerformed?.Invoke(ctx.ReadValue<float>()); }
}
