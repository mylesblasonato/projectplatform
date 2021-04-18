using System;
using UnityEngine;

public class PlatformerJump : MonoBehaviour
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] string _moveAxis;
    [SerializeField] string _jumpAxis;
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] ParticleSystem _gooVfx;
    [SerializeField] ParticleSystem _jumpExplosionGooVfx;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _groundCheckOffset;

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
    void InputCheck()
    {
        if (Input.GetButton(_jumpAxis) && _grounded)
        {
            _jumping = true;
        }
        if (Input.GetButtonUp(_jumpAxis))
        {
            _jumping = false;
        }
    }

    bool _grounded = true, _groundCheckLeft = false, _groundCheckRight = false;
    void GroundCheck()
    {
        _groundCheckLeft = SingleGroundCheck(transform.localPosition.x - _groundCheckOffset);
        _groundCheckRight = SingleGroundCheck(transform.localPosition.x + _groundCheckOffset);
        if (_groundCheckLeft || _groundCheckRight)
        {
            if(_wasJumping && _groundCheckDistance.Value > -1)
            {
                _jumpExplosionGooVfx.Play();
                _wasJumping = false;
            }

            SetGrounded(true);
            _OnGrounded.Invoke();
        }
        if (!_groundCheckLeft && !_groundCheckRight)
        {
            Invoke("CyoteTime", _cyoteTime.Value);
        }
    }

    public void SetGrounded(bool isGrounded) 
    {
        _grounded = isGrounded;
        _animator.SetBool("Grounded", _grounded); // land anim
    }

    bool SingleGroundCheck(float xPos)
    {
        return Physics2D.Raycast(
          new Vector2(xPos, transform.localPosition.y),
          new Vector2(0, _groundCheckDistance.Value),
          Mathf.Abs(_groundCheckDistance.Value),
          _groundLayer);
    }

    void ChangeGravity()
    {
        if (_groundCheckLeft || _groundCheckRight)
            _rb.gravityScale = 0;
        else if (!_groundCheckLeft && !_groundCheckRight)
            _rb.gravityScale = _gravity.Value * _fallMultiplier.Value;
    }

    #region UNITY
    void FixedUpdate()
    {
        if (_grounded)
        {
            _gooVfx.Play();
        }
        else
        {
            _gooVfx.Pause();
        }

        if (!_jumping) return;
        _gooVfx.Pause();
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        Jump();
        Invoke("JumpHeightController", _jumpHold.Value);
    }

    void Update()
    {
        InputCheck();
        GroundCheck();
        ChangeGravity();
        FallingCheck();
    }
    #endregion

    #region HELPERS
    void Jump() { _rb.AddForce(Vector2.up * _jumpForce.Value, ForceMode2D.Impulse); SetGrounded(false); _animator.SetBool("WallStick", false); _OnJump.Invoke(); }
    void JumpHeightController() { if (_jumping) _jumping = false; }
    void FallingCheck() => _animator.SetFloat("VelocityY", _rb.velocity.y);
    void CyoteTime() { _grounded = false; _animator.SetBool("Grounded", false); _wasJumping = true; } //jump anim
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector2(transform.localPosition.x - _groundCheckOffset, transform.localPosition.y), new Vector2(0, _groundCheckDistance.Value));
        Gizmos.DrawRay(new Vector2(transform.localPosition.x + _groundCheckOffset, transform.localPosition.y), new Vector2(0, _groundCheckDistance.Value));
    }
    #endregion
}