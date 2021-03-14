using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DCrouch : MonoBehaviour
{
    [SerializeField] BoxCollider2D _boxColliderStand, _boxColliderCrouch;
    [SerializeField] GameObject _playerSpriteTop;
    [SerializeField] Transform _crouchPos;

    Platformer2DJump _jumpMechanic;

    [HideInInspector] public bool _isCrouching = false;

    void Start()
    {
        _jumpMechanic = GetComponent<Platformer2DJump>();
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
                EventManager.Instance.GetComponent<EventManager>().Crouch();
            }
            else
            {
                _boxColliderStand.enabled = true;
                _boxColliderCrouch.enabled = false;
                _playerSpriteTop.transform.localPosition = Vector3.zero;
                EventManager.Instance.GetComponent<EventManager>().Stand();
            }
        }
    }

    public void Crouch(float isCrouching)
    {
        _isCrouching = (isCrouching > 0);
    }
}
