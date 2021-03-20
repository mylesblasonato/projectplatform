using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DJump : MonoBehaviour
{
    [SerializeField] SoFloat _jumpForce, _gravity, _drag, _airDrag, _dragSensitivity, _fallMultiplier, _jumpTimer, _jumpCount, _inputDirection, _cyoteTime, _crouchThreshold;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckRadius;
    [SerializeField] LayerMask _groundMask;

    [HideInInspector] public bool _isJumping = false;
     public bool _isGrounded = false;

    Platformer2DCrouch _crouchMechanic;
    Rigidbody2D _rb;
    int _currentJumpCount = 0;  
    float _currentJumpForce;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _crouchMechanic = GetComponent<Platformer2DCrouch>();
        _currentJumpForce = _jumpForce.Value;
    }

    private void FixedUpdate()
    {       
        Jumping();
        GroundCheck();
    }

    private void Update()
    {
        if(_isJumping)
            Invoke("StopJump", _jumpTimer.Value);
    }
    public void Jump(float isJumping)
    {
        if (isJumping > 0)
        {
                _isJumping = true;
                _crouchMechanic._isCrouching = false;
                EventManager.Instance.Jump();
           
        }
        else
            StopJump();
    }

    void Jumping()
    {
        if (_isJumping)
        {
            _rb.AddForce(Vector2.up * _currentJumpForce, ForceMode2D.Impulse);
        }
    }

    void StopJump() => _isJumping = false;

    void GroundCheck()
    {
        Invoke("DelayedGroundCheck", 0.1f);
        if (_isGrounded)
        {          
            EventManager.Instance.Land();
            _currentJumpCount = 0;
            if (Mathf.Abs(_rb.velocity.x) > _dragSensitivity.Value)
                _rb.drag = _drag.Value;
            else
                _rb.drag = 0f;
            _currentJumpForce = _jumpForce.Value;
        }
        else
        {
            _currentJumpCount++;
            _rb.drag = _drag.Value * _airDrag.Value;
            _rb.gravityScale = _gravity.Value;

            if (_crouchMechanic._crouchAxis >= _crouchThreshold.Value)
                _crouchMechanic._isCrouching = true;
        }
        _currentJumpForce -= _fallMultiplier.Value;
    }

    void DelayedGroundCheck()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundMask);        
    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
    #endregion
}
