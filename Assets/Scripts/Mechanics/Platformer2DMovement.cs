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
    Platformer2DShooting _platformerShoot;
    Platformer2DClimb _platformerClimb;

    float _velocity;
    Rigidbody2D _rb;
    bool _lookingLeft = false;

    public bool _isMoving = false;
    public bool _isRunning = false;
    public bool _isWalking = false;
    public bool _isWalkMode = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _platformerJump = GetComponent<Platformer2DJump>();
        _platformerCrouch = GetComponent<Platformer2DCrouch>();
        _platformerShoot = GetComponent<Platformer2DShooting>();
        _platformerClimb = GetComponent<Platformer2DClimb>();
        _velocity = _speed.Value;
    }

    public void Move(float inputDirection)
    {
        _inputDirection.Value = inputDirection;
        PlayAnimations();

        if (_platformerShoot._mouseFlip) return;
        FlipCharacter();
    }

    void FlipCharacter()
    {
        if (_platformerClimb._isClimbing) return;
        if (transform.eulerAngles.y == 180)
            _lookingLeft = true;
        if (transform.eulerAngles.y == 0)
            _lookingLeft = false;

        if (_inputDirection.Value < -_moveThreshold.Value && !_lookingLeft)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
            _lookingLeft = true;
        }

        if (_inputDirection.Value > _moveThreshold.Value && _lookingLeft)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            _lookingLeft = false;
        }
    }

    void PlayAnimations()
    {
        if (_platformerJump._isJumping || _platformerClimb._isClimbing) return;
        if (!_platformerCrouch._isCrouching && Mathf.Abs(_inputDirection.Value) > _moveThreshold.Value)
        {
            if (_isWalking)
                EventManager.Instance.TriggerEvent("OnWalk");
            if (_isRunning && !_isWalkMode)
                EventManager.Instance.TriggerEvent("OnRun");
        }
        else
        {
            EventManager.Instance.TriggerEvent("OnIdle");
        }
    }

    public void Walk(float isWalking)
    {
        if (_platformerJump._isJumping) return;
        if (isWalking == 1)
        {
            _isRunning = false;
            _isWalking = true;
            _isWalkMode = true;
        }
        else
        {
            _isWalking = false;
            _isWalkMode = false;
        }
    }

    private void FixedUpdate()
    {
        if (_platformerCrouch._isCrouching) return;

        if (!_platformerJump._isJumping)
            PlayAnimations();

        if(_inputDirection.Value > _moveThreshold.Value && !_platformerClimb._isClimbing)
            _rb.AddForce(new Vector2(1 * _velocity, 0));
        
        if (_inputDirection.Value < -_moveThreshold.Value && !_platformerClimb._isClimbing)
            _rb.AddForce(new Vector2(-1 * _velocity, 0));

        if (Mathf.Abs(_rb.velocity.x) == 0f && !_platformerJump._isJumping)
        {
            _isMoving = false;
            _isWalking = false;
        }
    }

    private void Update()
    {
        Movement();
        if (Mathf.Abs(_rb.velocity.x) < 0.5f && !_platformerJump._isJumping)
            EventManager.Instance.TriggerEvent("OnIdle");
    }

    private void Movement()
    {
        if (!_platformerCrouch._isCrouching || !_platformerJump._isJumping)
        {
            if (Mathf.Abs(_inputDirection.Value) > _moveThreshold.Value && Mathf.Abs(_inputDirection.Value) < 1)
            {
                _isWalking = true;
                _isMoving = true;
                _isRunning = false;
                _velocity = _speed.Value;
            }
            
            if (Mathf.Abs(_inputDirection.Value) >= 1 && !_isWalkMode)
            {
                _isWalking = false;
                _isRunning = true;
                _velocity = _speed.Value * _runMultiplier.Value;
            }
        }
    }
}
