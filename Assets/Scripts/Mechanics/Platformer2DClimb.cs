using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DClimb : MonoBehaviour
{
    [SerializeField] SoFloat _gravityScale;
    [SerializeField] Transform _grabPoint;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _range;

    Rigidbody2D _rb;
    RaycastHit2D _hitObject;
    public bool _isHoldingClimb = false;
    
    public bool _isClimbing = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
            _hitObject = hitObject;
            _isClimbing = true;
            _rb.velocity = Vector2.zero;
            _rb.gravityScale = 0;
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
