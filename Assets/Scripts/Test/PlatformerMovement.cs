using System;
using UnityEngine;
using UnityEngine.Events;

public class PlatformerMovement : MonoBehaviour
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] string _horizontalAxis;
    [SerializeField] float _moveSoundDuration;
    [SerializeField] string _moveSound;

    [Header("---SHARED---", order = 1)] //Scriptable Object Floats
    [SerializeField] SoFloat _acceleration;
    [SerializeField] SoFloat _maxSpeed;
    [SerializeField] SoFloat _deceleration;

    [Header("---EVENTS---", order = 2)] //EVENTS
    [SerializeField] GameEvent _OnMove;
    #endregion

    void Move(float horizontal)
    {
        if (Mathf.Abs(horizontal) > 0.2f)
            _rb.AddForce(new Vector2(horizontal * _acceleration.Value, 0));
        if (horizontal > 0 && !_facingRight || horizontal < 0 && _facingRight)
            Flip();
        if (Mathf.Abs(_rb.velocity.x) > _maxSpeed.Value)
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxSpeed.Value, _rb.velocity.y);
        if (_isGrounded)
        {
            _ac.SetFloat("Move", Mathf.Abs(_direction.x)); // move anim
        }
        if (Mathf.Abs(_rb.velocity.x) > 0.2f)
        {
            _OnMove?.Invoke();

            if (!_isInvokingFootsteps)
            {
                InvokeRepeating(nameof(Footsteps), 0f, _moveSoundDuration);
                _isInvokingFootsteps = true;
            }
        }
        else
        {
            if (_isInvokingFootsteps)
            {
                CancelInvoke("Footsteps");
                _isInvokingFootsteps = false;
            }

        }
    }

    bool _isInvokingFootsteps = false;
    void Footsteps()
    {
        _am.PlaySound2D(_moveSound);
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
    PlatformerAnimatorController _ac;
    Rigidbody2D _rb;
    AudioManager _am;

    void Awake()
    {
        _ac = GetComponent<PlatformerAnimatorController>();
        _rb = GetComponent<Rigidbody2D>();
        _am = FindAnyObjectByType<AudioManager>();
    }
    Vector2 _direction;
    void Update()
    {
        _direction = new Vector2(Input.GetAxis(_horizontalAxis), Input.GetAxis("Vertical"));
        ModifyPhysics();
    }
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