using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Platformer2DMovement : MonoBehaviour
{
    [SerializeField] SoFloat _speed, _airSpeedModifier, _runMultiplier;
    [SerializeField] Animator _animator, _animatorTop;
    [SerializeField] SoFloat _inputDirection;

    Platformer2DCrouch _platformerCrouch;
    Platformer2DJump _platformerJump;
    float _velocity;
    Rigidbody2D _rb;
    bool _isMoving = false;
    [HideInInspector] public bool _isRunning = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _platformerJump = GetComponent<Platformer2DJump>();
        _platformerCrouch = GetComponent<Platformer2DCrouch>();
        _velocity = _speed.Value;
    }

    public void Walk(float inputDirection)
    {
        _inputDirection.Value = inputDirection;
        RunCheck();
        if (_inputDirection.Value < 0)
            transform.localRotation = new Quaternion(0, 180f, 0, 0);
        if (_inputDirection.Value > 0)
            transform.localRotation = Quaternion.identity;
        if (_inputDirection.Value != 0)
            _isMoving = true;
        else
            _isMoving = false;
    }

    void RunCheck()
    {
        if (!_isRunning)
            EventManager.Instance.Walk();
        else
            EventManager.Instance.Run();
    }

    public void Run(float isRunning)
    {
        if (isRunning == 1)
        {
            _velocity *= _runMultiplier.Value;
            _isRunning = true;
        }
        else
        {
            _velocity =  _speed.Value;
            _isRunning = false;
        }
        RunCheck();
    }

    private void FixedUpdate()
    {
        if (_isMoving) 
            Move();
    }

    private void Update()
    {
        if (_rb.velocity.x == 0)
            EventManager.Instance.Idle();
    }

    private void Move()
    {
        if (!_platformerCrouch._isCrouching)
        {
            if (_platformerJump != null && _platformerJump._isJumping)
                _rb.velocity += new Vector2((_inputDirection.Value * _velocity) * _airSpeedModifier.Value, 0);
            else
                _rb.velocity += new Vector2(_inputDirection.Value * _velocity, 0);
        }
    }
}
