using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DClimb : MonoBehaviour
{
    [SerializeField] SoFloat _gravityScale, _hangOffset;
    [SerializeField] Transform _grabPoint;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _range;

    Platformer2DJump _platformerJump;

    Rigidbody2D _rb;
    RaycastHit2D _hitObject;
    bool _canFall = false;

    public bool _isHoldingClimb = false;   
    public bool _isClimbing = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _platformerJump = GetComponent<Platformer2DJump>();
    }

    private void Update()
    {
        if (_isHoldingClimb)
            ClimbRaycast();
        else
        {
            Drop();
        }
    }

    void Drop()
    {
        _isClimbing = false;
        _rb.gravityScale = _gravityScale.Value;

        if (_canFall)
        {
            EventManager.Instance.TriggerEvent("OnFall");
            _canFall = false;
        }

        if(_platformerJump._isGrounded)
            EventManager.Instance.TriggerEvent("OnLand");
    }

    void ClimbRaycast()
    {
        var origin = _grabPoint.position;
        var dest = _grabPoint.right;

        RaycastHit2D hitObject = Physics2D.Raycast(
            origin,
            dest,
            _range,
            _layerMask);

        if (hitObject)
        {
            if(!_isClimbing)
                transform.position = new Vector3(transform.position.x, transform.position.y - _hangOffset.Value, transform.position.z);
            
            _hitObject = hitObject;
            _isClimbing = true;
            _canFall = true;
            _rb.velocity = Vector2.zero;
            _rb.gravityScale = 0;
            EventManager.Instance.TriggerEvent("OnHang");
        }
        else
        {
            Drop();
        }
    }

    public void Climb(float isClimbing)
    {
        if (isClimbing > 0)
            _isHoldingClimb = true;
        else
            _isHoldingClimb = false;
    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(_grabPoint.position, _grabPoint.right * _range);
    }
    #endregion
}
