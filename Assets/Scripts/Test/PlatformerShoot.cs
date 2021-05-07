using System;
using UnityEngine;

public class PlatformerShoot : MonoBehaviour
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] string _shootAxis;
    [SerializeField] PlatformerGun _currentWeapon;
    [SerializeField] ObjectPoolingController _bulletPool;
    [SerializeField] Transform _gunMuzzle;

    [Header("---SHARED---", order = 1)] //Scriptable Object Floats
    [SerializeField] SoFloat _maxBulletsInPool;

    [Header("---EVENTS---", order = 2)] //EVENTS
    [SerializeField] GameEvent _OnFire;
    [SerializeField] GameEvent _OnReload;
    #endregion

    void Fire()
    {
        var bull = _bulletPool.InstantiateObject(_gunMuzzle);
        bull.GetComponent<BulletController>().ResetDirection(transform.right);
        _OnFire?.Invoke();
    }

    bool _canFire = true;
    void FireAgain()
    {
        _canFire = true;
    }

    #region UNITY    
    PlatformerAnimatorController _ac;
    Rigidbody2D _rb;
    void Awake()
    {
        _ac = GetComponent<PlatformerAnimatorController>();
        _rb = GetComponent<Rigidbody2D>();
    }

    bool _isShooting = false;
    void Update()
    {
        if (Input.GetAxis(_shootAxis) != 0 && _canFire)
        {
            _canFire = false;
            Fire();
            Invoke("FireAgain", _currentWeapon.FireRate);

            // ON AXIS DOWN
            if (!_isShooting)
            {
                _ac.SetBool("OnShoot", true);
                _isShooting = true;
            }
        }

        if (Input.GetAxisRaw(_shootAxis) == 0)
        {
            _ac.SetBool("OnShoot", false);
            _isShooting = false;
        }
    }
    #endregion

    #region HELPERS
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
       
    }
    #endregion
}