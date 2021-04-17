using System;
using UnityEngine;

public class PlatformerWallStick : MonoBehaviour
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] string _moveAxis;
    [SerializeField] string _jumpAxis;
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] ParticleSystem _gooVfx;
    [SerializeField] ParticleSystem _wallSplatVfx;
    [SerializeField] ParticleSystem _wallSplatExplosionVfx;

    [Header("---SHARED---", order = 1)] //Scriptable Object Floats
    [SerializeField] SoFloat _gravity;
    [SerializeField] SoFloat _wallCheckDistanceYoffset;
    [SerializeField] SoFloat _wallCheckDistance;
    [SerializeField] SoFloat _wallBackCheckDistance;

    [Header("---EVENTS---", order = 2)] //EVENTS
    [SerializeField] GameEvent _OnWall;
    #endregion

    RaycastHit2D _wallHit;
    bool _wallCheck = false;
    bool _backCheck = false;
    bool _jumping = false;
    void WallCheck()
    {
        _wallCheck = Physics2D.Raycast(
          new Vector2(transform.localPosition.x, transform.localPosition.y + _wallCheckDistanceYoffset.Value),
          new Vector2(transform.localRotation.y == 0 ? _wallCheckDistance.Value : -_wallCheckDistance.Value, 0),
          Mathf.Abs(_wallCheckDistance.Value),
          _wallLayer);

        _backCheck = Physics2D.Raycast(
          new Vector2(transform.localPosition.x, transform.localPosition.y + _wallCheckDistanceYoffset.Value),
          new Vector2(transform.localRotation.y == 0 ? -_wallBackCheckDistance.Value : _wallBackCheckDistance.Value, 0),
          Mathf.Abs(_wallBackCheckDistance.Value),
          _wallLayer);

        _wallHit = Physics2D.Raycast(
          new Vector2(transform.localPosition.x, transform.localPosition.y + _wallCheckDistanceYoffset.Value),
          new Vector2(transform.localRotation.y == 0 ? _wallCheckDistance.Value : -_wallCheckDistance.Value, 0),
          Mathf.Abs(_wallCheckDistance.Value),
          _wallLayer);
    }

    bool _grounded = false;
    public void OnGrounded() => _grounded = true;

    #region UNITY
    void Start()
    {
        _wallSplatVfx.enableEmission = false;
    }
    void Update()
    {
        WallCheck();

        if (_wallCheck)
        {          
            _wallSplatExplosionVfx.Play();
            _rb.gameObject.transform.eulerAngles = new Vector3(0, _rb.gameObject.transform.eulerAngles.y == 0 ? 180 : 0, 0);
            _OnWall.Invoke();
        }

        if (!_backCheck)
        {
            _grounded = false;
        }
        else
        {
            _rb.velocity = Vector2.zero;
            _rb.gravityScale = 0;
        }

        if (!_wallCheck && !_backCheck)
        {
            _gooVfx.enableEmission = true;
            _wallSplatVfx.enableEmission = false;
            _animator.SetBool("WallStick", false);

            if(_rb.velocity.x < -0.1f)
                transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (_rb.velocity.x > 0.1f)
                transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            _gooVfx.enableEmission = false;
            _wallSplatVfx.enableEmission = true;
            _animator.SetBool("WallStick", true);  
        }
    }
    #endregion

    #region HELPERS
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector2(transform.localPosition.x, transform.localPosition.y + _wallCheckDistanceYoffset.Value), new Vector2(transform.localRotation.y == 0 ? _wallCheckDistance.Value : -_wallCheckDistance.Value, 0));
        Gizmos.DrawRay(new Vector2(transform.localPosition.x, transform.localPosition.y + _wallCheckDistanceYoffset.Value), new Vector2(transform.localRotation.y == 0 ? -_wallBackCheckDistance.Value : _wallBackCheckDistance.Value, 0));
    }
    #endregion
}