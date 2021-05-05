using System;
using UnityEngine;

public class PlatformerWallStick : MonoBehaviour
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] string _moveAxis;
    [SerializeField] string _jumpAxis;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] ParticleSystem _gooVfx;
    [SerializeField] ParticleSystem _wallSplatVfx;
    [SerializeField] ParticleSystem _wallSplatExplosionVfx;
    [SerializeField] float _originalGroundCheckDistance; 
    [SerializeField] float _groundCheckDistanceOffset;

    [Header("---SHARED---", order = 1)] //Scriptable Object Floats
    [SerializeField] SoFloat _gravity;
    [SerializeField] SoFloat _jumpHorForce;
    [SerializeField] SoFloat _wallCheckDistanceYoffset;
    [SerializeField] SoFloat _wallCheckDistance;
    [SerializeField] SoFloat _wallBackCheckDistance;
    [SerializeField] SoFloat _groundCheckDistance;

    [Header("---EVENTS---", order = 2)] //EVENTS
    [SerializeField] GameEvent _OnWall;
    [SerializeField] GameEvent _OnOffWall;
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
    void JumpHorizontally() => _rb.AddForce(transform.right * _jumpHorForce.Value, ForceMode2D.Impulse);

    #region UNITY
    PlatformerAnimatorController _ac;
    Rigidbody2D _rb;
    void Awake()
    {
        _ac = GetComponent<PlatformerAnimatorController>();
        _rb = GetComponent<Rigidbody2D>();
    }
    void Start() => _wallSplatVfx.Pause();

    bool _jumpHor = false;
    void FixedUpdate()
    {
        if(_jumpHor)
        {
            JumpHorizontally();
            _jumpHor = false;
        }
    }

    void Update()
    {
        WallCheck();

        if (_wallCheck)
        {          
            _wallSplatExplosionVfx.Play();
            _gooVfx.Pause();
            _rb.gameObject.transform.eulerAngles = new Vector3(0, _rb.gameObject.transform.eulerAngles.y == 0 ? 180 : 0, 0);
            _OnWall.Invoke();
        }

        if (!_backCheck)
        {
            _wallSplatVfx.Pause();
            _grounded = false;
        }
        else
        {
            _wallSplatVfx.Play();
            _rb.velocity = Vector2.zero;
            _rb.gravityScale = 0;
            _gooVfx.Pause();
            _groundCheckDistance.Value = _groundCheckDistanceOffset;

            if (Input.GetButton(_jumpAxis))
            {
                _jumpHor = true;
            }
        }

        if (!_wallCheck && !_backCheck)
        {
            _groundCheckDistance.Value = _originalGroundCheckDistance;
            _OnOffWall?.Invoke();
            _ac.SetBool("WallStick", false);

            if(_rb.velocity.x < -0.1f)
                transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (_rb.velocity.x > 0.1f)
                transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            _ac.SetBool("WallStick", true);
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