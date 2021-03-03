using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DMovement : MonoBehaviour
{
    [SerializeField] float _speed, _runMultiplier = 1.5f, _jumpForce = 10f;
    [SerializeField] int _jumpCount = 3;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckRadius;
    [SerializeField] LayerMask _groundMask;

    float _velocity;
    Rigidbody2D _rb;
    float _inputDirection = 0f;
    bool _isMoving = false;
    bool _isRunning = false;
    bool _isGrounded = false;
    int _currentJumpCount = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _velocity = _speed;
    }

    public void Walk(float inputDirection)
    {
        _inputDirection = inputDirection;

        if (_inputDirection < 0)
            transform.localRotation = new Quaternion(0, 180f, 0, 0);
        if (_inputDirection > 0)
            transform.localRotation = Quaternion.identity;

        if (inputDirection != 0)
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
            _velocity = _speed;
            _isRunning = false;
        }
    }

    public void Jump(float isJumping)
    {
        if (_currentJumpCount < _jumpCount)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);          
        }
    }

    private void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundMask);
        if (_isGrounded) _currentJumpCount = 0;
        else _currentJumpCount++;
    }

    private void FixedUpdate()
    {
        if (_isMoving) Move(); 
    }

    private void Move()
    {
        _rb.velocity = new Vector2(_inputDirection * _velocity, _rb.velocity.y);       
    }

    // TOOLS / GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}
