using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DCrouch : MonoBehaviour
{
    [SerializeField] SoFloat _crouchDrag, _dashCrouchForce;
    [SerializeField] BoxCollider2D _boxColliderStand, _boxColliderCrouch;
    [SerializeField] GameObject _playerSpriteTop;
    [SerializeField] Transform _crouchPos;

    Platformer2DMovement _moveMechanic;
    Platformer2DJump _jumpMechanic;
    Rigidbody2D _rb;
    bool _crouchDash = false;

    public bool _isCrouching = false;

    void Start()
    {
        _moveMechanic = GetComponent<Platformer2DMovement>();
        _jumpMechanic = GetComponent<Platformer2DJump>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_jumpMechanic._isGrounded)
        {
            if (_isCrouching)
            {
                _boxColliderStand.enabled = false;
                _boxColliderCrouch.enabled = true;               
                _playerSpriteTop.transform.position = _crouchPos.position;
                EventManager.Instance.Crouch();

                if (_moveMechanic._isRunning)
                {
                    _rb.drag = _crouchDrag.Value;
                }
                else
                {
                    _rb.drag = 100f;
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

    void FixedUpdate()
    {
        if (_crouchDash)
        {
            _rb.AddForce(transform.right * _dashCrouchForce.Value * Time.deltaTime);
            _crouchDash = false;
        }
    }

    public void Crouch(float isCrouching)
    {
        _isCrouching = (isCrouching >= 0.9f);

        if (_moveMechanic._isRunning)
        {
            _crouchDash = true;
        }
    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Vector2.zero, Vector2.right);
    }
    #endregion
}
