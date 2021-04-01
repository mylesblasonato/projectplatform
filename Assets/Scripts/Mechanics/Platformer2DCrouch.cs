using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DCrouch : MonoBehaviour
{
    [SerializeField] SoFloat _crouchDrag, _dashCrouchForce, _crouchDashSpeedThreshold, _crouchThreshold, _crouchSlowDownSpeed, _slopeGravity;
    [SerializeField] SoFloat _inputDirection;
    [SerializeField] Collider2D _boxColliderStand, _boxColliderCrouch;
    [SerializeField] GameObject _playerSpriteTop;
    [SerializeField] Transform _crouchPos;
    [SerializeField] LayerMask _slopeMask, _slideMask;

    Platformer2DMovement _moveMechanic;
    Platformer2DJump _jumpMechanic;
    Rigidbody2D _rb;
    bool _canCrouchDash = false;
    RaycastHit2D _slope;
    bool _isSloping = false;
    bool _objectAbove = false;
    bool _slidingUnderObject = false;

    public float _crouchAxis = 0;
    public bool _isCrouching = false;

    void Start()
    {
        _moveMechanic = GetComponent<Platformer2DMovement>();
        _jumpMechanic = GetComponent<Platformer2DJump>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        SlideRaycastUp();
        SlopeRaycastAndCheck();
        if (!_jumpMechanic._isGrounded) return;
        CrouchDown();
        Stand();
    }

    void FixedUpdate()
    {
        Slide();
        SlopeRotation();

        if (_objectAbove && _isCrouching)
        {
            _slidingUnderObject = true;
            _rb.AddForce(new Vector2((_rb.velocity.x + _dashCrouchForce.Value) * _inputDirection.Value, 0), ForceMode2D.Impulse);
        }

        if (_slidingUnderObject && !_objectAbove)
        {
            _rb.velocity = Vector2.zero;
            _slidingUnderObject = false;
        }       
    }

    void Slide()
    {
        if (_jumpMechanic._isGrounded &&
            _isCrouching &&
            _canCrouchDash &&
            _moveMechanic._isMoving
            && Mathf.Abs(_rb.velocity.x) > _crouchDashSpeedThreshold.Value)
        {
            _rb.AddForce(new Vector2((_rb.velocity.x + _dashCrouchForce.Value) * _inputDirection.Value, 0), ForceMode2D.Impulse);
            _canCrouchDash = false;
        }
    }

    void SlopeRaycastAndCheck()
    {
        _slope = Physics2D.Raycast(
            Vector2.zero + new Vector2(transform.localPosition.x, transform.localPosition.y),
            new Vector2(0, -1),
            2f,
            _slopeMask);

        if (_slope && !_isSloping)
            Slope();

        if (!_slope)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            _isSloping = false;
        }
    }

    void SlideRaycastUp()
    {
        _objectAbove = Physics2D.Raycast(
            Vector2.zero + new Vector2(transform.localPosition.x, transform.localPosition.y),
            new Vector2(0, 1),
            2f,
            _slideMask);
    }

    void SlopeRotation() // change rot of player when on slope
    {
        if (_isSloping)
        {
            transform.right.Normalize();
            transform.eulerAngles = (transform.right.x > 0) ?
                new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -45) :
                new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 45);
        }
    }

    void Slope()
    {
        _rb.gravityScale = _slopeGravity.Value;
        float slopeAngle = Vector2.Angle(_slope.point, _slope.normal + new Vector2(transform.position.x, transform.position.y));
        if (slopeAngle > 0 && !_isSloping)
        {
            Invoke("IsSloping", 0);
            transform.right.Normalize();
        }
    }

    void IsSloping() => _isSloping = true;

    public void Stand()
    {
        if (_isCrouching && _jumpMechanic._isGrounded) return;
        _isCrouching = false;
        _boxColliderStand.enabled = true;
        _boxColliderCrouch.enabled = false;
        _playerSpriteTop.transform.localPosition = Vector3.zero;
        EventManager.Instance.TriggerEvent("OnStand");
    }

    public void CrouchDown()
    {
        if (!_isCrouching && !_jumpMechanic._isGrounded) return;
        _boxColliderStand.enabled = false;
        _boxColliderCrouch.enabled = true;
        _playerSpriteTop.transform.position = _crouchPos.position;
        _playerSpriteTop.transform.localRotation = _crouchPos.localRotation;
        EventManager.Instance.TriggerEvent("OnCrouch");
        _rb.drag = _crouchDrag.Value;
    }

    public void Crouch(float isCrouching)
    {
        if (!_jumpMechanic._isGrounded) return;
        _crouchAxis = Mathf.Abs(isCrouching);
        _isCrouching = (_crouchAxis >= _crouchThreshold.Value);
        if (!_isCrouching)
            _canCrouchDash = true;
    }

    #region HELPERS
    #endregion
}
