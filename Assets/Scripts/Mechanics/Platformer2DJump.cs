using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DJump : MonoBehaviour
{
    [SerializeField] SoFloat _jumpForce, _gravity, _drag, _airDrag, _dragSensitivity, _fallMultiplier, _jumpTimer, _jumpCount, _inputDirection;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckRadius;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] Animator _animator;

    Rigidbody2D _rb;
    int _currentJumpCount = 0;
    bool _isGrounded = false;
    float _currentJumpForce;

    [HideInInspector] public bool _isJumping = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentJumpForce = _jumpForce.Value;
    }

    private void FixedUpdate()
    {
        GroundCheck(_inputDirection.Value > 0 && _rb.velocity.x < 0 || _inputDirection.Value < 0 && _rb.velocity.x > 0);
        Jumping();
    }

    public void Jump(float isJumping)
    {
        if (isJumping == 1)
        {
            if (_currentJumpCount < _jumpCount.Value)
            {
                _isJumping = true;
                Invoke("StopJump", _jumpTimer.Value);
            }
        }
        else
        {
            StopJump();
        }
    }

    void Jumping()
    {
        if (_isJumping)
        {
            _rb.AddForce(Vector2.up * _currentJumpForce, ForceMode2D.Impulse);          
        }
    }

    void StopJump()
    {
        _isJumping = false;
    }

    void GroundCheck(bool changingDirections)
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundMask);
        if (_isGrounded)
        {
            _currentJumpCount = 0;
            if (Mathf.Abs(_rb.velocity.x) > _dragSensitivity.Value)
                _rb.drag = _drag.Value;
            else
                _rb.drag = 0f;
            _rb.gravityScale = 0;

            _currentJumpForce = _jumpForce.Value;
        }
        else
        {
            _currentJumpCount++;
            _rb.drag = _drag.Value * _airDrag.Value;
            _rb.gravityScale = _gravity.Value;
        }

        _currentJumpForce -= _fallMultiplier.Value;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}
