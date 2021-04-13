using System;
using UnityEngine;

public class PlatformerJump : MonoBehaviour
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] string _jumpAxis;
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _groundCheckDistance, _groundCheckOffset;

    [Header("---SHARED---", order = 1)] //Scriptable Object Floats
    [SerializeField] SoFloat _jumpForce;
    [SerializeField] SoFloat _jumpHold;
    [SerializeField] SoFloat _gravity;
    [SerializeField] SoFloat _fallMultiplier;
    [SerializeField] SoFloat _cyoteTime;

    [Header("---EVENTS---", order = 2)] //EVENTS
    [SerializeField] GameEvent _OnGrounded;
    [SerializeField] GameEvent _OnJump;    
    #endregion

    bool _jumping = false;
    void InputCheck()
    {
        if (Input.GetButton(_jumpAxis) && _grounded)
            _jumping = true;
        if (Input.GetButtonUp(_jumpAxis))
            _jumping = false;
    }

    bool _grounded = false, _groundCheckLeft = false, _groundCheckRight = false;
    void GroundCheck()
    {
        _groundCheckLeft = SingleGroundCheck(transform.localPosition.x - _groundCheckOffset);
        _groundCheckRight = SingleGroundCheck(transform.localPosition.x + _groundCheckOffset);
        if (_groundCheckLeft || _groundCheckRight)
        {
            _grounded = true;
            _OnGrounded.Invoke();
        }
        if (!_groundCheckLeft && !_groundCheckRight)
            Invoke("CyoteTime", _cyoteTime.Value);
    }

    bool SingleGroundCheck(float xPos)
    {
        return Physics2D.Raycast(
          new Vector2(xPos, transform.localPosition.y),
          new Vector2(0, _groundCheckDistance),
          Mathf.Abs(_groundCheckDistance),
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
        if (!_jumping) return;
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
    void Jump() => _rb.AddForce(Vector2.up * _jumpForce.Value, ForceMode2D.Impulse);
    void JumpHeightController() { if (_jumping) _jumping = false; }
    void FallingCheck() => _animator.SetFloat("VelocityY", _rb.velocity.y);
    void CyoteTime() { _grounded = false; _OnJump.Invoke(); } //jump anim
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector2(transform.localPosition.x - _groundCheckOffset, transform.localPosition.y), new Vector2(0, _groundCheckDistance));
        Gizmos.DrawRay(new Vector2(transform.localPosition.x + _groundCheckOffset, transform.localPosition.y), new Vector2(0, _groundCheckDistance));
    }
    #endregion
}