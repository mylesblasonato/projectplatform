using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DCrouch : MonoBehaviour
{
    [SerializeField] SoFloat _crouchDrag, _dashCrouchForce, _crouchDashSpeedThreshold, _crouchThreshold;
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

    public bool _isCrouching = false;

    void Start()
    {
        _moveMechanic = GetComponent<Platformer2DMovement>();
        _jumpMechanic = GetComponent<Platformer2DJump>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //if (_isCrouching)
        {
            _slope = Physics2D.Raycast(
                Vector2.zero + new Vector2(transform.localPosition.x, transform.localPosition.y),
                new Vector2(0, -1),
                1000f,
                _slopeMask); 

            if (_slope && !_isSloping)
            {
                Slope();
            }

            if (!_slope && _isSloping)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                _isSloping = false;
            }
        }

        if (_jumpMechanic._isGrounded)
        {
            if (_isCrouching)
            {
                _boxColliderStand.enabled = false;
                _boxColliderCrouch.enabled = true;
                _playerSpriteTop.transform.position = _crouchPos.position;
                _playerSpriteTop.transform.localRotation = _crouchPos.localRotation;
                EventManager.Instance.Crouch();

                if (_moveMechanic._isRunning)
                {
                    _rb.drag = _crouchDrag.Value;
                }
                else
                {
                    if (_rb.velocity.x < 0)
                        _rb.velocity = new Vector2(_rb.velocity.x + 0.05f, 0);
                    if (_rb.velocity.x > 0)
                        _rb.velocity = new Vector2(_rb.velocity.x - 0.05f, 0);
                }
            }
            else
            {
                _boxColliderStand.enabled = true;
                _boxColliderCrouch.enabled = false;
                _playerSpriteTop.transform.localPosition = Vector3.zero;
                EventManager.Instance.Stand();
            }
        }
    }

    void Slope()
    {
        Debug.Log(Mathf.Abs(Vector2.Dot(transform.up, _slope.normal + new Vector2(transform.localPosition.x, transform.localPosition.y))));
        if (Mathf.Abs(Vector2.Dot(transform.up, _slope.normal + new Vector2(transform.localPosition.x, transform.localPosition.y))) < 1)
        {
            _isSloping = true;
            transform.eulerAngles = new Vector3(0, 0, -45);
        }
    }

    void FixedUpdate()
    {
        if (_canCrouchDash && _moveMechanic._isRunning && _isCrouching && _moveMechanic._isMoving && Mathf.Abs(_rb.velocity.x) > _crouchDashSpeedThreshold.Value)
        {
            _rb.velocity = Vector2.zero;
            _rb.AddForce(transform.right * _dashCrouchForce.Value * Time.deltaTime, ForceMode2D.Impulse);
            _canCrouchDash = false;
        }
    }

    public void Crouch(float isCrouching)
    {
        _isCrouching = (isCrouching >= _crouchThreshold.Value);      

        if (!_isCrouching)
        {
            _canCrouchDash = true;
        }
    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        if (_slope)
        {
            Gizmos.DrawLine(_slope.point, _slope.normal + new Vector2(transform.position.x, transform.position.y));
        }
    }
    #endregion
}
