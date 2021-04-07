using System;
using UnityEngine;

public class PlatformerJump : MonoBehaviour
{
    [SerializeField] string _jumpAxis;
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _groundCheckDistance, _groundCheckOffset;
    [SerializeField] SoFloat _jumpForce, _jumpHold, _gravity, _fallMultiplier, _cyoteTime;   

    bool _groundCheckLeft = false, _groundCheckRight = false, _grounded = false, _jumping = false;

    void FixedUpdate()
    {
        if (_jumping)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            Jump();
            Invoke("JumpHeightController", _jumpHold.Value);
        }
    }

    void Update()
    {
        InputCheck();
        GroundCheck();
        ChangeGravity();
        FallingCheck();
    }

    void InputCheck()
    {
        if (Input.GetButton(_jumpAxis) && _grounded)
            _jumping = true;
        if (Input.GetButtonUp(_jumpAxis))
            _jumping = false;
    }

    void Jump()
    {
        _rb.AddForce(Vector2.up * _jumpForce.Value, ForceMode2D.Impulse);
    }

    void GroundCheck()
    {
        _groundCheckLeft = SingleGroundCheck(transform.localPosition.x - _groundCheckOffset);
        _groundCheckRight = SingleGroundCheck(transform.localPosition.x + _groundCheckOffset);
        if (_groundCheckLeft || _groundCheckRight)
        {
            _grounded = true;
            EventManager.Instance.TriggerEvent("OnGrounded"); // land anim
        }
        if (!_groundCheckLeft && !_groundCheckRight)
        {
            Invoke("CyoteTime", _cyoteTime.Value);
        }
    }

    void CyoteTime()
    {
        _grounded = false;
        EventManager.Instance.TriggerEvent("OnJump"); // jump anim
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
        {
            _rb.gravityScale = 0;
        }
        else if (!_groundCheckLeft && !_groundCheckRight)
        {
            _rb.gravityScale = _gravity.Value * _fallMultiplier.Value;
        }
    }

    void FallingCheck()
    {
        _animator.SetFloat("VelocityY", _rb.velocity.y);
    }

    void JumpHeightController()
    {
        if(_jumping)
            _jumping = false;
    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector2(transform.localPosition.x - _groundCheckOffset, transform.localPosition.y), new Vector2(0, _groundCheckDistance));
        Gizmos.DrawRay(new Vector2(transform.localPosition.x + _groundCheckOffset, transform.localPosition.y), new Vector2(0, _groundCheckDistance));
    }
    #endregion
}
