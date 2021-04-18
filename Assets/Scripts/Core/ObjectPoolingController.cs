using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingController : MonoBehaviour
{
    [SerializeField] GameObject _objectPrefab;
    [SerializeField] int _maxCount;

    Queue<GameObject> _objects;

    void Awake()
    {
        _objects = new Queue<GameObject>();
    }

    void Start()
    {
        for (int i = 0; i < _maxCount; i++)
        {
            var newObject = Instantiate(_objectPrefab, transform);
            newObject.SetActive(false);
            _objects.Enqueue(newObject);
        }
    }

    public GameObject InstantiateObject(Transform spawnPosition)
    {
        var curObj = _objects.Dequeue();
        curObj.transform.position = spawnPosition.position;
        curObj.SetActive(true);
        _objects.Enqueue(curObj);
        return curObj;
    }
    public void DestroyObject(GameObject curObj)
    {
        curObj.SetActive(false);
    }
}
