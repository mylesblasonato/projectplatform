using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DShooting : MonoBehaviour
{
    [SerializeField] Transform _equippedGun;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] bool _mouseAim = false, _mouseFlip = false;
    [SerializeField] SoFloat _inputDirection;

    Rigidbody2D _rb;
    Camera _main;
    RaycastHit2D _hitObject;

    private void Start()
    {
        _lineRenderer.enabled = false;
        _rb = GetComponent<Rigidbody2D>();
        _main = Camera.main;
    }

    private void Update()
    {
        if (_mouseFlip)
        {
            if (_main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
                transform.localRotation = new Quaternion(0, 180f, 0, 0);
            if (_main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
                transform.localRotation = Quaternion.identity;
        }
    }

    public void Shoot(float ctx)
    {
        if (Mathf.Abs(ctx) >= 1)
        {
            _lineRenderer.enabled = true;
            EventManager.Instance.Shoot();
            _equippedGun.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();
            var origin = new Vector2(_equippedGun.GetChild(0).transform.position.x, _equippedGun.GetChild(0).transform.position.y);
            var dest = new Vector2();

            if (!_mouseAim)
                dest = (transform.localRotation.y == 0) ? transform.right : -transform.right;
            else
                dest = _main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            RaycastHit2D hitObject = Physics2D.Raycast(
                origin,
                dest,
                _equippedGun.GetComponent<Weapon>().Range,
                _equippedGun.GetComponent<Weapon>().LayerMask);

            if (hitObject)
            {
                _hitObject = hitObject;
                _lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
                _lineRenderer.SetPosition(1, new Vector3(hitObject.transform.position.x, 0, 0));

                hitObject.transform.GetComponent<Rigidbody2D>().AddForce(-hitObject.normal * _equippedGun.GetComponent<Weapon>().Power);
                
                
            }
            else
            {
                _lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
                _lineRenderer.SetPosition(1, dest * 500);
            }

            Invoke("TurnOffLine", 0.1f);
        }
        else
        {
            EventManager.Instance.StopShoot();
        }
    }

    void TurnOffLine()
    {
        _lineRenderer.enabled = false;
        BloodVfx();
    }

    void BloodVfx()
    {
        if (_hitObject.transform.gameObject.layer != 9) return;
        _hitObject.transform.GetChild(0).position = _hitObject.point;
        _hitObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        if (!_mouseAim)
            Gizmos.DrawRay(_equippedGun.GetChild(0).transform.position, _equippedGun.right);
        else
            Gizmos.DrawRay(_equippedGun.GetChild(0).transform.position, _main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

    }
    #endregion
}
