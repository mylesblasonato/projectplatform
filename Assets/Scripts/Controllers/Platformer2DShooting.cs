using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DShooting : MonoBehaviour
{
    [SerializeField] Transform _equippedGun;
    [SerializeField] GameObject _lineRendererPrefab, _bloodVfxPrefab, _dustVfxPrefab;    
    [SerializeField] SoFloat _inputDirection;
    [SerializeField] bool _mouseAim = false;

    LineRenderer _lineRenderer;
    Rigidbody2D _rb;
    Camera _main;
    RaycastHit2D _hitObject;

    public bool _mouseFlip = false;

    private void Start()
    {
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
            _lineRenderer = GameObject.Instantiate(_lineRendererPrefab).GetComponent<LineRenderer>();
            EventManager.Instance.TriggerEvent("OnShoot");
            _equippedGun.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();
            var origin = _equippedGun.transform.position;
            var dest = new Vector2();

            if (!_mouseAim)
                dest = transform.right;
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
                _lineRenderer.SetPosition(0, new Vector2(origin.x + (transform.right.x * 0.3f), origin.y));
                _lineRenderer.SetPosition(1, hitObject.point);
                hitObject.transform.GetComponent<Rigidbody2D>().AddForce(-hitObject.normal * _equippedGun.GetComponent<Weapon>().Power);
                Invoke("TurnOffLine", 0.1f);
            }
            else
            {
                _lineRenderer.SetPosition(0, new Vector2(origin.x + (transform.right.x * 0.3f), origin.y));
                _lineRenderer.SetPosition(1, dest * 500);
            }
        }
        else
        {
            EventManager.Instance.TriggerEvent("OnStopShoot");
        }
    }

    void TurnOffLine()
    {
        BloodVfx();
    }

    void BloodVfx()
    {
        if (_hitObject.transform.GetComponent<EnemyController>() == null)
        {
            var dust = GameObject.Instantiate(_dustVfxPrefab);
            dust.transform.position = _hitObject.point;
            if (_hitObject.normal.x > 0)
                dust.transform.eulerAngles = new Vector3(0, 0, 0);
            else
                dust.transform.eulerAngles = new Vector3(0, 180f, 0);
            dust.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            var blood = GameObject.Instantiate(_bloodVfxPrefab);
            blood.transform.position = _hitObject.point;
            if (_hitObject.normal.x > 0)
                blood.transform.eulerAngles = new Vector3(0, 0, 0);
            else
                blood.transform.eulerAngles = new Vector3(0, 180f, 0);
            blood.GetComponent<ParticleSystem>().Play();
        }
    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        if (_mouseAim)
            Gizmos.DrawRay(_equippedGun.GetChild(0).transform.position, _main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

    }
    #endregion
}
