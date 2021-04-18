using UnityEngine;

[CreateAssetMenu(menuName = "2D Platformer/Gun", fileName = "New Gun")]
public class PlatformerGun : ScriptableObject
{
    [SerializeField] string _name = "Default Name";
    public string Name => _name;

    [SerializeField] Sprite _sprite;
    public Sprite Sprite => _sprite;

    [SerializeField] GameObject _bulletPrefab;
    public GameObject BulletPrefab => _bulletPrefab;

    [SerializeField] string _poolParent;
    public string PoolParent => _poolParent;

    [SerializeField] ParticleSystem _shootExplosionVfx;
    public ParticleSystem ShootExplosionVfx => _shootExplosionVfx;

    [SerializeField] LayerMask _enemyMask;
    public LayerMask _EnemyMask => _enemyMask;

    [SerializeField] float _power = 100f;
    public float Power => _power;

    [SerializeField] float _fireRate = 1f;
    public float FireRate => _fireRate;

    [SerializeField] float _reloadSpeed = 1f;
    public float ReloadSpeed => _reloadSpeed;

    [SerializeField] int _maxClips = 3;
    public int MaxClips => _maxClips;

    [SerializeField] float _clipSize = 12;
    public float ClipSize => _clipSize;

    float _currentAmmo = 0;
    public float CurrentAmmo => _currentAmmo;
}
