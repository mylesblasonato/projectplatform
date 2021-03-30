using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _acceleration, _speed, _deceleration, _jumpForce, _jumpHightMax, _groundCheckDistance, _airDrag;

    bool _changeDirection = false;
    bool _groundCheck = false;
    bool _isJumping = false;

    void FixedUpdate()
    {
        //if (_groundCheck)
            Jump();          
        Move();
    }

    void Update()
    {
        GroundCheck();
    }

    void GroundCheck()
    {
        _groundCheck = Physics2D.Raycast(
          new Vector2(transform.localPosition.x, transform.localPosition.y),
          new Vector2(0, _groundCheckDistance),
          Mathf.Abs(_groundCheckDistance),
          _groundLayer);

        if (_groundCheck)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _isJumping = false;
        }
    }

    void Jump()
    {
        if (Input.GetButton("Fire1"))
        {
            if (_rb.velocity.y < _jumpHightMax && !_isJumping)
            {
                _rb.AddForce(new Vector2(0, _jumpForce));
            }
            else
            {
                _rb.drag = _airDrag;
                _isJumping = true;
            }
        }     
    }

    void Move()
    {
        _animator.SetFloat("Move", Mathf.Abs(Input.GetAxis("Horizontal")));
        if (Mathf.Abs(_rb.velocity.x) < _speed)
        {
            _rb.AddForce(new Vector2((_rb.velocity.x + (Input.GetAxis("Horizontal") * _acceleration)), 0));
             _rb.drag = 0;
            _changeDirection = false;
            //rotation
            if (Input.GetAxis("Horizontal") > 0.02f)
                transform.eulerAngles = new Vector3(0, 0, 0);
            else if (Input.GetAxis("Horizontal") < -0.02f)
                transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < 1)
            _rb.drag = _deceleration;
        if (Mathf.Abs(_rb.velocity.x) == 0)
            _changeDirection = true;
    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector2(transform.localPosition.x, transform.localPosition.y), new Vector2(0, _groundCheckDistance));
    }
    #endregion
}
