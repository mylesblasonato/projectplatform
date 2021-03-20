using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DCrouch : MonoBehaviour
{
    [SerializeField] SoFloat _crouchDrag, _dashCrouchForce, _crouchDashSpeedThreshold, _crouchThreshold, _crouchSlowDownSpeed, _slopeGravity;
    [SerializeField] BoxCollider2D _boxColliderStand, _boxColliderCrouch;
    [SerializeField] GameObject _playerSpriteTop;
    [SerializeField] Transform _crouchPos;
    [SerializeField] LayerMask _slopeMask;

    Platformer2DMovement _moveMechanic;
    Platformer2DJump _jumpMechanic;
    Rigidbody2D _rb;
    bool _canCrouchDash = false;
    RaycastHit2D _slope;
    bool _isSloping = false;

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
        SlopeRaycastAndCheck();
        if (!_jumpMechanic._isGrounded) return;
        CrouchDown();
        Stand();
    }

    //

    // Slows down the player when crouching
    void ManualFriction()
    {
        if (Mathf.Abs(_rb.velocity.x) > 0.2f)
            _rb.velocity = new Vector2(_rb.velocity.x + (-transform.right.normalized.x * _crouchSlowDownSpeed.Value), 0);
        else
            _rb.velocity = Vector2.zero;
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

    void Slope()
    {
        _rb.gravityScale = _slopeGravity.Value;
        float slopeAngle = Vector2.Angle(_slope.point, _slope.normal + new Vector2(transform.position.x, transform.position.y));
        if (slopeAngle > 0 && !_isSloping)
        {
            _isSloping = true;
            transform.right.Normalize();
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -45 * transform.right.x);
        }
    }

    void FixedUpdate()
    {
        if (_jumpMechanic._isGrounded && 
            _canCrouchDash && 
            _moveMechanic._isRunning && 
            _isCrouching && 
            _moveMechanic._isMoving 
            && Mathf.Abs(_rb.velocity.x) > _crouchDashSpeedThreshold.Value)
        {
            _rb.velocity = Vector2.zero;
            _rb.AddForce(transform.right * _dashCrouchForce.Value, ForceMode2D.Impulse);
            _canCrouchDash = false;
        }

        if (_isSloping)
        {
            transform.right.Normalize();
            transform.eulerAngles = (transform.right.x > 0) ?
                new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -45) :
                new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 45);
        }
    }

    public void Stand()
    {
        if (_isCrouching) return;
        _boxColliderStand.enabled = true;
        _boxColliderCrouch.enabled = false;
        _playerSpriteTop.transform.localPosition = Vector3.zero;
        EventManager.Instance.Stand();
    }

    public void CrouchDown()
    {
        if (!_isCrouching && !_jumpMechanic._isGrounded) return;
        _boxColliderStand.enabled = false;
        _boxColliderCrouch.enabled = true;
        _playerSpriteTop.transform.position = _crouchPos.position;
        _playerSpriteTop.transform.localRotation = _crouchPos.localRotation;
        EventManager.Instance.Crouch();
        if (_moveMechanic._isRunning)
            _rb.drag = _crouchDrag.Value;
        else
            if(_isCrouching)
                ManualFriction();
    }

    public void Crouch(float isCrouching)
    {
        if (!_jumpMechanic._isGrounded) return;
        _crouchAxis = isCrouching;
        _isCrouching = (isCrouching >= _crouchThreshold.Value);      
        if (!_isCrouching)
            _canCrouchDash = true;
    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        //if (_slope)
            //Gizmos.DrawLine(_slope.point, _slope.normal + new Vector2(transform.position.x, transform.position.y));
    }
    #endregion
}
