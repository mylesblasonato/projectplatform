using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] string _playerTag;
    [SerializeField] float _bulletSpeed = 1;
    [SerializeField] float _duration = 3f;
    [SerializeField] ObjectPoolingController _bulletPool;

    GameObject _player;
    float _elapsedTime = 0;
    public Vector3 _direction;

    #region UNITY
    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(_playerTag);
    }

    void Start()
    {
        _elapsedTime = _duration;
        _direction = _player.transform.right;
    }

    void Update()
    {
        if (_elapsedTime > 0)
        {
            transform.position += _direction * _bulletSpeed * Time.deltaTime;
            _elapsedTime -= Time.deltaTime;
        }
        else
        {
            _bulletPool.DestroyObject(this.gameObject);
            _elapsedTime = _duration;
        }
    }
    #endregion

    #region HELPERS
    public void ResetDirection(Vector2 dir) => _direction = dir;
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        
    }
    #endregion
}
