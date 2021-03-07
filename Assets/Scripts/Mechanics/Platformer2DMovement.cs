using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Platformer2DMovement : MonoBehaviour
{
    [SerializeField] float _speed, _runMultiplier = 1.5f, _jumpForce = 10f, _gravity = 10f, _drag = 10f, _fallMultiplier = 3f, _jumpTimer = 0.5f;
    [SerializeField] int _jumpCount = 3;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckRadius;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] Animator _animator;

    float _velocity;
    Rigidbody2D _rb;
    float _inputDirection = 0f;
    bool _isMoving = false;
    bool _isRunning = false;
    bool _isGrounded = false;
    bool _isJumping = false;
    int _currentJumpCount = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _velocity = _speed;
    }

    public void Walk(float inputDirection)
    {
        _inputDirection = inputDirection;
        _animator.SetFloat("IdleToWalk", Mathf.Abs(_inputDirection));

        if (_inputDirection < 0)
            transform.localRotation = new Quaternion(0, 180f, 0, 0);
        if (_inputDirection > 0)
            transform.localRotation = Quaternion.identity;

        if (_inputDirection != 0)
            _isMoving = true;
        else
            _isMoving = false;
    }

    public void Run(float isRunning)
    {
        if (isRunning != 0)
        {
            _velocity *= _runMultiplier;
            _isRunning = true;
        }
        else
        {
            _velocity =  _speed;
            _isRunning = false;
        }
    }

    public void Jump(float isJumping)
    {
        if (_currentJumpCount < _jumpCount)
        {
            _isJumping = true;
        }

        if(isJumping == 0)
            _isJumping = false;

        Invoke("StopJump", _jumpTimer);
    }

    void StopJump()
    {
        _isJumping = false;
    }

    private void Update()
    {
        bool changingDirections = (_inputDirection > 0 && _rb.velocity.x < 0 || _inputDirection < 0 && _rb.velocity.x > 0);

        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundMask);
        if (_isGrounded)
        {
            _currentJumpCount = 0;

            if (Mathf.Abs(_inputDirection) < 0.4f || changingDirections)
            {
                _rb.drag = _drag;
            }
            else
                _rb.drag = 0f;

            _rb.gravityScale = 0;
        }
        else
        { 
            _currentJumpCount++;
            _rb.drag = _drag * 0.15f;
            _rb.gravityScale = _gravity;
        }

        if (_isJumping)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            //_isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        if (_isMoving) Move(); 
    }

    private void Move()
    {
        if (Mathf.Abs(_inputDirection) == 1)
        {
            _rb.velocity = new Vector2(_inputDirection * _velocity, _rb.velocity.y);
        }
    }

    // TOOLS / GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}
