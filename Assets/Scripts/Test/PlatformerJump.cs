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

    bool _groundCheckLeft = false, _groundCheckRight = false;

    void FixedUpdate()
    {
        if (Input.GetButton(_jumpAxis) && (_groundCheckLeft || _groundCheckRight))
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
        EventManager.Instance.TriggerEvent("OnJump");
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * _jumpForce.Value, ForceMode2D.Impulse);
        _animator.SetBool("Jumping", true);
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
    }

    void ChangeGravity()
    {
        if (_groundCheckLeft || _groundCheckRight)
        {
            _rb.gravityScale = 0;
            EventManager.Instance.TriggerEvent("OnGrounded");
        }

        if (!_groundCheckLeft && !_groundCheckRight)
        {
            _rb.gravityScale = _gravity.Value;
            JumpHeightController();
        }
    }

    void FallingCheck()
    {
        if (!_groundCheckLeft && !_groundCheckRight)
        {
            if (_rb.velocity.y <= 0)
                _animator.SetBool("Jumping", false);
            else
                _animator.SetBool("Jumping", true);
        }
    }

    void JumpHeightController()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = _gravity.Value * (_fallMultiplier.Value / 2f);
        }
        else if (_rb.velocity.y > 0 && !Input.GetButton(_jumpAxis))
        {
            _rb.gravityScale = _gravity.Value * _fallMultiplier.Value;
        }
    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector2(transform.localPosition.x - _groundCheckOffset, transform.localPosition.y), new Vector2(0, _groundCheckDistance));
        Gizmos.DrawRay(new Vector2(transform.localPosition.x + _groundCheckOffset, transform.localPosition.y), new Vector2(0, _groundCheckDistance));
    }
    #endregion
}
