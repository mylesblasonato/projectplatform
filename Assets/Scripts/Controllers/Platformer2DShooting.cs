using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DShooting : MonoBehaviour
{
    [SerializeField] Transform _equippedGun;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] bool _mouseAim = false, _mouseFlip = false;

    private void Start()
    {
        _lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (_mouseFlip)
        {
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
                transform.localRotation = new Quaternion(0, 180f, 0, 0);
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
                transform.localRotation = Quaternion.identity;
        }
    }

    public void Shoot(float ctx)
    {
        if (Mathf.Abs(ctx) >= 1)
        {
            _lineRenderer.enabled = true;
            _equippedGun.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();           

            var origin = new Vector2(_equippedGun.GetChild(0).transform.position.x, _equippedGun.GetChild(0).transform.position.y);
            var dest = new Vector2();

            if (!_mouseAim)
                dest = _equippedGun.right;
            else
                dest = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            RaycastHit2D hitObject = Physics2D.Raycast(
                origin,
                dest,
                _equippedGun.GetComponent<Weapon>().Range,
                _equippedGun.GetComponent<Weapon>().LayerMask);

            if (hitObject)
            {
                _lineRenderer.SetPosition(0, _equippedGun.GetChild(0).GetChild(0).transform.position);
                _lineRenderer.SetPosition(1, hitObject.transform.position);

                hitObject.transform.GetComponent<Rigidbody2D>().AddForce(-hitObject.normal * _equippedGun.GetComponent<Weapon>().Power);
                hitObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            }
            else
            {
                _lineRenderer.SetPosition(0, _equippedGun.GetChild(0).GetChild(0).transform.position);
                _lineRenderer.SetPosition(1, _equippedGun.GetChild(0).GetChild(0).transform.right * 100f);
            }

            Invoke("TurnOffLine", 0.1f);
        }
    }

    void TurnOffLine()
    {
        _lineRenderer.enabled = false;
    }
    
    // TOOLS / GIZMOS
    private void OnDrawGizmos()
    {
        if (!_mouseAim)
            Gizmos.DrawRay(_equippedGun.GetChild(0).transform.position, _equippedGun.right);
        else
            Gizmos.DrawRay(_equippedGun.GetChild(0).transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

    }
}
