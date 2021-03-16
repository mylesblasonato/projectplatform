using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Platformer2DMovement : MonoBehaviour
{
    [SerializeField] SoFloat _speed, _airSpeedModifier, _runMultiplier, _moveThreshold, _idleThreshold;
    [SerializeField] Animator _animator, _animatorTop;
    [SerializeField] SoFloat _inputDirection;

    Platformer2DCrouch _platformerCrouch;
    Platformer2DJump _platformerJump;
    float _velocity;
    Rigidbody2D _rb;
    bool _lookingLeft = false;

    public bool _isMoving = false;
    public bool _isRunning = false;

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

        if (transform.eulerAngles.y == 180)
            _lookingLeft = true;
        else
            _lookingLeft = false;

        if (inputDirection < 0 && !_lookingLeft)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
            _lookingLeft = true;
        } 
        else if (inputDirection > 0 && _lookingLeft)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            _lookingLeft = false;
        }

        if (_inputDirection.Value != 0 && Mathf.Abs(_rb.velocity.x) > _idleThreshold.Value)
            _isMoving = true;     
    }

    void RunCheck()
    {
        if (!_platformerCrouch._isCrouching)
        {
            if (!_isRunning)
                EventManager.Instance.Walk();
            else
                EventManager.Instance.Run();
        }
        else
        {
            EventManager.Instance.Idle();
        }
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
        Move();

        if (Mathf.Abs(_rb.velocity.x) < 0.5f)
            _isMoving = false;
    }

    private void Update()
    {
        if (Mathf.Abs(_rb.velocity.x) < 0.5f)
            EventManager.Instance.Idle();
    }

    private void Move()
    {
        if (!_platformerCrouch._isCrouching && Mathf.Abs(_inputDirection.Value) > _moveThreshold.Value)
        {
            if (_platformerJump != null && _platformerJump._isJumping)
                _rb.velocity += new Vector2((_inputDirection.Value * _velocity) * _airSpeedModifier.Value, 0);
            else
                _rb.velocity += new Vector2(_inputDirection.Value * _velocity, 0);
        }
    }
}
