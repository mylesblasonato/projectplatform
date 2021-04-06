using System;
using UnityEngine;

public class PlatformerJump : MonoBehaviour
{
    [SerializeField] string _jumpAxis;
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] SoFloat _jumpForce, _gravity, _fallMultiplier;
    [SerializeField] float _groundCheckDistance, _groundCheckOffset;

    bool _groundCheckLeft = false, _groundCheckRight = false, _grounded = false;

    void FixedUpdate()
    {
        if (Input.GetButton(_jumpAxis) && _grounded)
        {
            Jump();
        }
    }

    void Update()
    {
        GroundCheck();
        ChangeGravity();
        FallingCheck();
    }

    void Jump()
    {
        _rb.AddForce(Vector2.up * _jumpForce.Value, ForceMode2D.Impulse);
    }

    void GroundCheck()
    {
        _groundCheckLeft = Physics2D.Raycast(
          new Vector2(transform.localPosition.x - _groundCheckOffset, transform.localPosition.y),
          new Vector2(0, _groundCheckDistance),
          Mathf.Abs(_groundCheckDistance),
          _groundLayer);

        _groundCheckRight = Physics2D.Raycast(
          new Vector2(transform.localPosition.x + _groundCheckOffset, transform.localPosition.y),
          new Vector2(0, _groundCheckDistance),
          Mathf.Abs(_groundCheckDistance),
          _groundLayer);

        if (_groundCheckLeft || _groundCheckRight)
            EventManager.Instance.TriggerEvent("OnGrounded");

        if(!_groundCheckLeft && !_groundCheckRight)
            EventManager.Instance.TriggerEvent("OnJump");

        if (_groundCheckLeft || _groundCheckRight)
            _grounded = true;
        else if (!_groundCheckLeft && !_groundCheckRight)
            _grounded = false;

    }

    void ChangeGravity()
    {
        if (_groundCheckLeft || _groundCheckRight)
        {
            _rb.gravityScale = _gravity.Value;
        }
        else if (!_groundCheckLeft && !_groundCheckRight)
        {
            _rb.gravityScale = _gravity.Value * (_fallMultiplier.Value / 2f);
            JumpHeightController();
        }
    }

    void FallingCheck()
    {
        _animator.SetFloat("VelocityY", _rb.velocity.y);
    }

    void JumpHeightController()
    {

    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector2(transform.localPosition.x - _groundCheckOffset, transform.localPosition.y), new Vector2(0, _groundCheckDistance));
        Gizmos.DrawRay(new Vector2(transform.localPosition.x + _groundCheckOffset, transform.localPosition.y), new Vector2(0, _groundCheckDistance));
    }
    #endregion
}
