﻿using System;
using UnityEngine;
using UnityEngine.Animations;

public class PlatformerJump : MonoBehaviour
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] string _moveAxis;
    [SerializeField] string _jumpAxis;
    [SerializeField] ParticleSystem _gooVfx;
    [SerializeField] ParticleSystem _jumpExplosionGooVfx;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _groundCheckOffset;
    [SerializeField] float _landingSpeedCheck;
    [SerializeField] string _landSound;

    [Header("---SHARED---", order = 1)] //Scriptable Object Floats
    [SerializeField] SoFloat _jumpForce;
    [SerializeField] SoFloat _jumpHold;
    [SerializeField] SoFloat _gravity;
    [SerializeField] SoFloat _fallMultiplier;
    [SerializeField] SoFloat _cyoteTime;
    [SerializeField] SoFloat _groundCheckDistance;

    [Header("---EVENTS---", order = 2)] //EVENTS
    [SerializeField] GameEvent _OnGrounded;
    [SerializeField] GameEvent _OnJump;
    #endregion

    bool _wasJumping = false;
    bool _jumping = false;
    float _jumpDuration = 0f;
    void InputCheck()
    {
        if (Input.GetButton(_jumpAxis) && _grounded || Input.GetButton(_jumpAxis) && _isOnWall)
        {
            _jumping = true;
            _jumpDuration = 0;
            _OnJump.Invoke();
        }
        
        if (Input.GetButtonDown(_jumpAxis))
        {
            _jumpDuration = 0;
        }

        if (Input.GetButtonUp(_jumpAxis))
        {
            _jumping = false;
        }
    }

    bool _grounded = true, _groundCheckLeft = false, _groundCheckRight = false; bool _hasLanded = false;
    void GroundCheck()
    {
        _groundCheckLeft = SingleGroundCheck(transform.localPosition.x - _groundCheckOffset);
        _groundCheckRight = SingleGroundCheck(transform.localPosition.x + _groundCheckOffset);

        if (_groundCheckLeft || _groundCheckRight)
        {
            if (_wasJumping && _groundCheckDistance.Value > -1)
            {
                _jumpExplosionGooVfx.Play();
                _wasJumping = false;
            }

            if (!_hasLanded) // ✅ Play landing sound/effect only once
            {
                _hasLanded = true;
                _OnGrounded.Invoke(); // Invoke your landing event just once
                _am.PlaySound2D(_landSound); // Optional: replace with your landing sound
            }

            SetGrounded(true);
        }
        else
        {
            _hasLanded = false; // You're airborne again, allow future landing sound
            Invoke(nameof(CyoteTime), _cyoteTime.Value);
        }
    }

    void ChangeGravity()
    {
        if (_groundCheckLeft || _groundCheckRight)
            _rb.gravityScale = 0;
        else if (!_groundCheckLeft && !_groundCheckRight)
            _rb.gravityScale = _gravity.Value * _fallMultiplier.Value;
    }

    #region UNITY
    PlatformerAnimatorController _ac;
    Rigidbody2D _rb;
    AudioManager _am;
    void Awake()
    {
        _ac = GetComponent<PlatformerAnimatorController>();
        _rb = GetComponent<Rigidbody2D>();
        _am = FindAnyObjectByType<AudioManager>();
    }
    void FixedUpdate()
    {
        if (_grounded)
        {
            _gooVfx.Play();
        }
        else
        {
            _jumpDuration += Time.deltaTime;
            _gooVfx.Pause();
        }

        if (!_jumping)
        {
            return;
        }

        _gooVfx.Pause();
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        Jump();
        Invoke("JumpHeightController", _jumpHold.Value);
    }

    void Update()
    {
        _ac?.SetFloat("HangTime", _jumpDuration);
        InputCheck();
        GroundCheck();
        ChangeGravity();
        FallingCheck();
    }
    #endregion

    #region HELPERS
    void Jump()
    {
        _rb.AddForce(Vector2.up * _jumpForce.Value, ForceMode2D.Impulse);
        SetGrounded(false);
        _ac?.SetBool("WallStick", false); 
    }
    void JumpHeightController() { if (_jumping) _jumping = false; }
    void FallingCheck()
    {
        _ac?.SetFloat("VelocityY", _rb.velocity.y);
    }
    void CyoteTime() { _grounded = false; _ac?.SetBool("Grounded", false); _wasJumping = true; } //jump anim

    bool _isOnWall = false;
    public void SetOnWall(bool isOnWall)
    {
        _isOnWall = isOnWall;
    }

    public void SetGrounded(bool isGrounded)
    {
        _grounded = isGrounded;
        if (_isOnWall) _ac?.SetBool("Grounded", false); 
        else _ac?.SetBool("Grounded", _grounded); // land anim
    }

    bool SingleGroundCheck(float xPos)
    {
        return Physics2D.Raycast(
          new Vector2(xPos, transform.localPosition.y),
          new Vector2(0, _groundCheckDistance.Value),
          Mathf.Abs(_groundCheckDistance.Value),
          _groundLayer);
    }
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector2(transform.localPosition.x - _groundCheckOffset, transform.localPosition.y), new Vector2(0, _groundCheckDistance.Value));
        Gizmos.DrawRay(new Vector2(transform.localPosition.x + _groundCheckOffset, transform.localPosition.y), new Vector2(0, _groundCheckDistance.Value));
    }
    #endregion
}