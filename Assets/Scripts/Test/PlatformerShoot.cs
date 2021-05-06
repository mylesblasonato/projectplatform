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
        _ac.SetBool("OnShoot", true);
        _OnFire?.Invoke();
    }

    bool _canFire = true;
    void FireAgain()
    {
        _canFire = true;
        _ac.SetBool("OnShoot", false);
    }

    #region UNITY    
    PlatformerAnimatorController _ac;
    Rigidbody2D _rb;
    void Awake()
    {
        _ac = GetComponent<PlatformerAnimatorController>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetAxis(_shootAxis) >= 1 && _canFire)
        {
            _canFire = false;
            Fire();
            Invoke("FireAgain", _currentWeapon.FireRate);
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