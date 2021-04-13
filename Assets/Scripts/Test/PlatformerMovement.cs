using System;
using UnityEngine;
using UnityEngine.Events;

public class PlatformerMovement : MonoBehaviour
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] string _horizontalAxis;
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody2D _rb;

    [Header("---SHARED---", order = 1)] //Scriptable Object Floats
    [SerializeField] SoFloat _acceleration;
    [SerializeField] SoFloat _maxSpeed;
    [SerializeField] SoFloat _deceleration;
    #endregion

    Vector2 _direction;
    void Update()
    {
        _direction = new Vector2(Input.GetAxis(_horizontalAxis), Input.GetAxis("Vertical"));
        _animator.SetBool("Grounded", _isGrounded); // land anim
        ModifyPhysics();
    }
    
    void Move(float horizontal)
    {
        if (Mathf.Abs(horizontal) > 0.2f)
            _rb.AddForce(new Vector2(horizontal * _acceleration.Value, 0));
        if (horizontal > 0 && !_facingRight || horizontal < 0 && _facingRight)
            Flip();
        if (Mathf.Abs(_rb.velocity.x) > _maxSpeed.Value)
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxSpeed.Value, _rb.velocity.y);
        if (_isGrounded)
            _animator.SetFloat("Move", Mathf.Abs(_direction.x)); // move anim
    }

    void ModifyPhysics()
    {
        var changingDirections = (_direction.x > 0 && _rb.velocity.x < 0) || (_direction.x < 0 && _rb.velocity.x > 0);
        if (Mathf.Abs(_direction.x) < 1f || changingDirections)
            _rb.drag = _deceleration.Value;
    }

    bool _facingRight = true;
    void Flip()
    {
        _facingRight = !_facingRight;
        transform.rotation = Quaternion.Euler(0, _facingRight ? 0 : 180, 0);
    }

    #region UNITY
    void FixedUpdate() => Move(_direction.x);
    #endregion

    #region HELPERS
    bool _isGrounded = false;
    public void OnGrounded() => _isGrounded = true;
    public void OnJump() => _isGrounded = false;
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        //Gizmos.DrawRay(new Vector2(transform.localPosition.x, transform.localPosition.y), new Vector2(0, _groundCheckDistance));
    }
    #endregion
}